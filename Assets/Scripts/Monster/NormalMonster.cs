using DesignPattern;
using UnityEngine;
using UnityEngine.AI;

public class NormalMonster : Monster, IDamageable
{
    private bool _isActivateControl = true;
    private bool _canTracking = true;
    [SerializeField] private int MaxHp;
    private ObservableProperty<int> CurrentHp = new();
    private ObservableProperty<bool> IsMoving = new();
    private ObservableProperty<bool> IsAttacking = new();
    private ObservableProperty<bool> IsDead = new();
    private Animator _animator;

    [Header("Config Navmesh")]
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _targetTransform;

    private void Awake() => Init();

    private void Update()
    {
        HandleControl();
        Death(IsDead.Value);
    }

    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;
        _animator = GetComponent<Animator>();
        CurrentHp.Value = MaxHp;
        IsDead.Value = true;
    }

    private void HandleControl()
    {
        if (!_isActivateControl) return;

        HandleMove();
    }

    private void HandleMove()
    {
        if (_targetTransform == null) return;

        if (_canTracking)
        {
            _navMeshAgent.SetDestination(_targetTransform.position);
        }
        _navMeshAgent.isStopped = !_canTracking;
        IsMoving.Value = _canTracking;
        _animator.SetBool("IsMove", _canTracking);
    }

    public void TakeDamage(int value)
    {
        CurrentHp.Value -= value;
        if (CurrentHp.Value <= 0)
        {
            IsDead.Value = false;
        }
    }

    private void Death(bool value)
    {
        _animator.SetBool("IsAlive", value);
    }
}
