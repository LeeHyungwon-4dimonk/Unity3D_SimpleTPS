using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] private float _attackRange;
    [SerializeField] private int _shootDamage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private AudioClip _shootSFX;
    [SerializeField] private ParticleSystem _fireParticles;

    private CinemachineImpulseSource _impulse;
    private Camera _camera;

    private bool _canShoot { get => _currentCount <= 0; }
    private float _currentCount;

    private void Awake() => Init();

    private void Update() => HandleCanShoot();

    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    public bool Shoot()
    {
        if (!_canShoot) return false;
        
        PlayShootSound();
        PlayCameraEffect();
        PlayShootEffect();
        _currentCount = _shootDelay;

        RaycastHit hit;
        IDamageable target = RayShoot(out hit);

        if(!hit.Equals(default))
        {
            PlayFireEffect(hit.point, Quaternion.LookRotation(hit.normal));
        }

        if (target == null) return true;

        target.TakeDamage(_shootDamage);
        //Debug.Log("공격받음!");

        return true;
    }

    private void HandleCanShoot()
    {
        if(_canShoot) return;

        _currentCount -= Time.deltaTime;
    }

    private IDamageable RayShoot(out RaycastHit hitTarget)
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        //Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 100, Color.red, 3);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
        {
            hitTarget = hit;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                return ReferenceRegistry.GetProvider(hit.collider.gameObject).GetAs<NormalMonster>() as IDamageable;
            }
        }

        hitTarget = hit;
        return null;
    }

    private void PlayFireEffect(Vector3 position, Quaternion rotation)
    {
        Instantiate(_fireParticles, position, rotation);
    }

    private void PlayShootSound()
    {
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }

    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }

    private void PlayShootEffect()
    {
        // TODO : 총구 화염 효과. 파티클로 구현해보기
    }
}
