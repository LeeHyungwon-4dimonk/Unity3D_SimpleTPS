using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    private bool _canTracking = false;
    public bool CanTracking { get { return _canTracking; } }

    private Transform _targetTransform = null;
    public Transform TargetTransform { get { return _targetTransform; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTracking = true;
            _targetTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _canTracking = false;
            _targetTransform = null;
        }
    }
}
