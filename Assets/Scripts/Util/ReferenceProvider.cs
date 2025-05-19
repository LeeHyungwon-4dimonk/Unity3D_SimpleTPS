using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class ReferenceProvider : MonoBehaviour
{
    [SerializeField] private Component _component;

    private void Awake() => ReferenceRegistry.Register(this);

    private void OnDestroy() => ReferenceRegistry.Unregister(this);

    public T GetAs<T>() where T : Component
    {
        return _component as T;
    }
}
