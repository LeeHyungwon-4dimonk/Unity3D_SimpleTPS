using UnityEngine;

// TODO : 참조 생성용 임시 네임스페이스 참조. 작업물 병합 시 삭제예정
using PlayerMovement = A_Test.PlayerMovement;

public class PlayerController : MonoBehaviour
{
    public bool isControlActive { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;

    [SerializeField] GameObject _aimCamera;
    private GameObject _mainCamera;

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
        _mainCamera = Camera.main.gameObject;
    }

    private void HandlePlayerControl()
    {
        if (!isControlActive) return;
        HandleMovement();
        HandleAiming();
    }

    private void HandleMovement()
    {
        // TODO : Movement 병합 시 기능 추가 예정
    }

    private void HandleAiming()
    {
        _status.IsAming.Value = Input.GetKey(_aimkey);
    }

    public void SubscribeEvents()
    {
        _status.IsAming.Subscribe(value => SetActiveCamera(value));
    }

    public void UnsubscribeEvents()
    {
        _status.IsAming.UnSubscribe(value => SetActiveCamera(value));
    }

    private void SetActiveCamera(bool value)
    {
        _aimCamera.SetActive(value);
        _mainCamera.SetActive(!value);
    }
}
