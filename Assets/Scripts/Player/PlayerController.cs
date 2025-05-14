using UnityEngine;

// TODO : ���� ������ �ӽ� ���ӽ����̽� ����. �۾��� ���� �� ��������
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
    /// �ʱ�ȭ�� �Լ�, ��ü ���� �� �ʿ��� �ʱ�ȭ �۾�
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
        // TODO : Movement ���� �� ��� �߰� ����
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
