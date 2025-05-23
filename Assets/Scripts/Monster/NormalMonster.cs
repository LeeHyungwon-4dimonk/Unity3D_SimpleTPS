using DesignPattern;
using UnityEngine;
using UnityEngine.AI;

public class NormalMonster : Monster, IDamageable
{
    private bool _isActivateControl = true;
    //private bool _canTracking = true;
    [SerializeField] private int MaxHp;
    private ObservableProperty<int> CurrentHp = new();
    private ObservableProperty<bool> IsMoving = new();
    //private ObservableProperty<bool> IsAttacking = new();
    private ObservableProperty<bool> IsAlive = new();
    private Animator _animator;

    [SerializeField] private DetectArea _detectArea;
    [SerializeField] private GameObject _monsterAttack;

    [Header("Config Navmesh")]
    private NavMeshAgent _navMeshAgent;
    //[SerializeField] private Transform _targetTransform;

    private void Awake() => Init();

    private void Update() => HandleControl();


    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;
        _animator = GetComponent<Animator>();
        CurrentHp.Value = MaxHp;
        IsAlive.Value = true;
    }

    private void HandleControl()
    {        
        if (!_isActivateControl) return;
        HandleMove();
        Death(IsAlive.Value);
    }

    private void HandleMove()
    {
        if (_detectArea.TargetTransform == null) 
        _animator.SetBool("IsMove", _detectArea.CanTracking);

        if (_detectArea.CanTracking)
        {
            _navMeshAgent.SetDestination(_detectArea.TargetTransform.position);
        }
        _navMeshAgent.isStopped = !_detectArea.CanTracking;
        IsMoving.Value = _detectArea.CanTracking;
        _animator.SetBool("IsMove", _detectArea.CanTracking);
    }

    public void TakeDamage(int value)
    {
        CurrentHp.Value -= value;
        if (CurrentHp.Value <= 0)
        {
            IsAlive.Value = false;
            Death(IsAlive.Value);
            _monsterAttack.SetActive(false);
        }
    }
    
    private void Death(bool value)
    {
        _animator.SetBool("IsAlive", value);
        if (value == false)
        {
            _isActivateControl = false;
            _navMeshAgent.speed = 0;
        }
    }
}
