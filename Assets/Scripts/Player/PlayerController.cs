using TMPro.EditorUtilities;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isControlActive { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;

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

        _hpUI.SetImageFillAmount(1);
        _status.CurrentHp.Value = _status.MaxHp;
    }

    private void HandlePlayerControl()
    {
        if (!isControlActive) return;

        HandleMovement();
        HandleAiming();
        HandleShooting();

        if(Input.GetKey(KeyCode.Q))
        {
            TakeDamage(1);
        }
        if(Input.GetKey(KeyCode.E))
        {
            RecoveryHP(1);
        }
    }

    private void HandleShooting()
    {
        if (_status.IsAming.Value && Input.GetKey(_shootKey))
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
            Vector3 input = _movement.GetInputDirection();
            _animator.SetFloat("X", input.x);
            _animator.SetFloat("Z", input.z);
        }
    }

    private void HandleAiming()
    {
        _status.IsAming.Value = Input.GetKey(_aimkey);
    }

    public void TakeDamage(int value)
    {
        _status.CurrentHp.Value -=value;

        if (_status.CurrentHp.Value <= 0) Dead();
    }

    public void RecoveryHP(int value)
    {
        int hp = _status.CurrentHp.Value + value;
        _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
    }

    public void Dead()
    {
        Debug.Log("플레이어 사망 처리");
    }

    public void SubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAniation);
        _status.IsAming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAming.Subscribe(SetAimAnimation);
        _status.IsAttacking.Subscribe(SetAttackAnimation);
        _status.CurrentHp.Subscribe(SetHPUIGuage);
    }

    public void UnsubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAniation);
        _status.IsAming.UnSubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAming.UnSubscribe(SetAimAnimation);
        _status.IsAttacking.UnSubscribe(SetAttackAnimation);
        _status.CurrentHp.UnSubscribe(SetHPUIGuage);
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
