using System.Collections.Generic;
using DesignPattern;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _bgmSource;

    [SerializeField] private List<AudioClip> _bgmList = new();

    [SerializeField] private SFXController _sfxPrefab;

    private ObjectPool _sfxPool;

    private void Awake() => Init();

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();

        _sfxPool = new ObjectPool(transform, _sfxPrefab, 10);
    }

    public void BgmPlay(int index)
    {
        if (0 <= index && index < _bgmList.Count)
        {
            _bgmSource.Stop();
            _bgmSource.clip = _bgmList[index];
            _bgmSource.Play();
        }
    }

    public SFXController GetSFX()
    {
        PooledObject po = _sfxPool.PopPool();
        return po as SFXController;
    }
}
