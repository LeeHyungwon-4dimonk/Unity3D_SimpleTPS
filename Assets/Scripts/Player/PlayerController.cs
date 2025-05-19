using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour, IDamageable
{
    public bool isControlActive { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;
    private InputAction _aimInputAction;
    private InputAction _shootInputAction;

    [SerializeField] private Image _aimImage;
    [SerializeField] private Animator _aimAnimator;
    [SerializeField] GameObject _aimCamera;
    [SerializeField] private Gun _gun;
    [SerializeField] private HpGuageUI _hpUI;

    [SerializeField] private KeyCode _aimkey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;

    private void Awake() => Init();
    private void OnEnable() => SubscribeEvents();
    private void Update() => HandlePlayerControl();
    private void OnDisable() => UnsubscribeEvents();

    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _aimImage = _aimAnimator.GetComponent<Image>();
        _aimInputAction = GetComponent<PlayerInput>().actions["Aim"];
        _shootInputAction = GetComponent<PlayerInput>().actions["Shoot"];

        _hpUI.SetImageFillAmount(1);
        _status.CurrentHp.Value = _status.MaxHp;
    }

    private void HandlePlayerControl()
    {
        if (!isControlActive) return;

        HandleMovement();
        //HandleAiming();
        OnShoot();
        //Debug.DrawRay(_aimCamera.transform.position, _aimCamera.transform.forward * 100, Color.red, 2);       
    }

    //private void HandleShooting()
    public void OnShoot()
    {
        // _shootInputAction.WasPressedThisFrame(); => 이번 프레임에 눌렀는가? (GetKeyDown)
        // _shootInputAction.WasReleasedThisFrame(); => 이번 프레임에 떼어졌는가? (GetKeyUp)
        // _shootInputAction.IsPressed(); => 지금 눌려있는가? (GetKey)


        //if (_status.IsAming.Value && Input.GetKey(_shootKey))
        if (_status.IsAming.Value && _shootInputAction.IsPressed())
        {
            _status.IsAttacking.Value = _gun.Shoot();
        }
        else
        {
            _status.IsAttacking.Value = false;
        }
    }

    private void HandleMovement()
    {
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAming.Value) moveSpeed = _status.WalkSpeed;
        else moveSpeed = _status.RunSpeed;

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        // 몸체의 회전
        Vector3 avatarDir;
        if (_status.IsAming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);

        if (_status.IsAming.Value)
        {
            //Vector2 input = _movement.InputDirection;
            //_animator.SetFloat("X", input.x);
            //_animator.SetFloat("Z", input.y);

            _animator.SetFloat("X", _movement.InputDirection.x);
            _animator.SetFloat("Z", _movement.InputDirection.y);
        }
    }


    private void HandleAiming(InputAction.CallbackContext ctx)
    {
        //_status.IsAming.Value = Input.GetKey(_aimkey);
        _status.IsAming.Value = ctx.started;

        // ctx.started => 키 입력이 시작됐는지 판별
        // ctx.performed => 키 입력이 진행중인지 판별
        // ctx.canceled => 키 입력이 취소됐는지(떼어졌는지) 판별
    }


    public void TakeDamage(int value)
    {
        _status.CurrentHp.Value -= value;

        if (_status.CurrentHp.Value <= 0) Dead();
    }

    public void RecoveryHP(int value)
    {
        int hp = _status.CurrentHp.Value + value;
        _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
    }

    public void Dead()
    {
        _animator.SetTrigger("IsAlive");
        isControlActive = false;
    }

    public void SubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAniation);
        _status.IsAming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAming.Subscribe(SetAimAnimation);
        _status.IsAttacking.Subscribe(SetAttackAnimation);
        _status.CurrentHp.Subscribe(SetHPUIGuage);

        // inputs---
        _aimInputAction.Enable();
        _aimInputAction.started += HandleAiming;
        _aimInputAction.canceled += HandleAiming;
    }

    public void UnsubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAniation);
        _status.IsAming.UnSubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAming.UnSubscribe(SetAimAnimation);
        _status.IsAttacking.UnSubscribe(SetAttackAnimation);
        _status.CurrentHp.UnSubscribe(SetHPUIGuage);

        // inputs---
        _aimInputAction.Disable();
        _aimInputAction.started -= HandleAiming;
        _aimInputAction.canceled -= HandleAiming;
    }

    private void SetAimAnimation(bool value)
    {
        if (!_aimImage.enabled) _aimImage.enabled = true;
        _animator.SetBool("IsAim", value);
        _aimAnimator.SetBool("IsAim", value);
    }
    private void SetMoveAniation(bool value) => _animator.SetBool("IsMove", value);
    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);

    private void SetHPUIGuage(int currentHp) => _hpUI.SetImageFillAmount((float)currentHp / _status.MaxHp);

}
