using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isControlActive { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;

    [SerializeField] GameObject _aimCamera;
    [SerializeField] private Gun _gun;

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
    }

    private void HandlePlayerControl()
    {
        if (!isControlActive) return;

        HandleMovement();
        HandleAiming();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if(_status.IsAming.Value && Input.GetKey(_shootKey))
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

        // ¸öÃ¼ÀÇ È¸Àü
        Vector3 avatarDir;
        if (_status.IsAming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);

        if(_status.IsAming.Value)
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

    public void SubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAniation);
        _status.IsAming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAming.Subscribe(SetAimAnimation);
        _status.IsAttacking.Subscribe(SetAttackAnimation);
    }

    public void UnsubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAniation);
        _status.IsAming.UnSubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAming.UnSubscribe(SetAimAnimation);
        _status.IsAttacking.UnSubscribe(SetAttackAnimation);
    }
    
    private void SetAimAnimation(bool value) => _animator.SetBool("IsAim", value);
    private void SetMoveAniation(bool value) => _animator.SetBool("IsMove", value);
    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);
}
