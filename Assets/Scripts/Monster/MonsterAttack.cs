using UnityEngine;
using UnityEngine.AI;

public class MonsterAttack : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public float _attackTime;

    [SerializeField] private int _monsterDamage;

    private void Awake() => Init();

    private void Update()
    {
        if (_attackTime < 0)
        {
            _attackTime += Time.deltaTime;
            if (_attackTime >= 0)
            {
                _attackTime = 0;
                _navMeshAgent.speed = 2;
            }
        }
    }

    private void Init()
    {
        _animator = GetComponentInParent<Animator>();
        _attackTime = 0;
        _navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger("IsAttack");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _attackTime += Time.deltaTime;
            _navMeshAgent.speed = 0;

            if (_attackTime >= 2.2)
            {
                _attackTime = -1.2f;
                _animator.SetTrigger("IsAttack");
            }
            
            if (_attackTime <= 1.77 && _attackTime >= 1.75 && other != null && other.CompareTag("Player"))
            {
                IDamageable playerAttack = ReferenceRegistry.GetProvider(other.gameObject).GetAs<PlayerController>() as IDamageable;
                playerAttack.TakeDamage(_monsterDamage);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _attackTime = -2.7f + _attackTime;
        }
    }
}
