using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isControlActive { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;

    [SerializeField] GameObject _aimCamera;

    [SerializeField] private KeyCode _aimkey = KeyCode.Mouse1;

    private void Awake() => Init();
    private void OnEnable() => SubscribeEvents();
    private void Update() => HandlePlayerControl();
    private void OnDisable() => UnsubscribeEvents();

    /// <summary>
    /// 초기화용 함수, 객체 생성 시 필요한 초기화 작업
    /// </summary>
    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();        
    }

    private void HandlePlayerControl()
    {
        if (!isControlActive) return;
        HandleMovement();
        HandleAiming();
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
    }

    private void HandleAiming()
    {
        _status.IsAming.Value = Input.GetKey(_aimkey);
    }

    public void SubscribeEvents()
    {
        _status.IsAming.Subscribe(_aimCamera.gameObject.SetActive);
    }

    public void UnsubscribeEvents()
    {
        _status.IsAming.UnSubscribe(_aimCamera.gameObject.SetActive);
    }
}
