using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGuageUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Transform _cameraTransform;

    private void Awake() => Init();
    private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);

    private void Init()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void SetImageFillAmount(float value)
    {
        _image.fillAmount = value;
    }

    private void SetUIForwardVector(Vector3 target)
    {
        transform.forward = target;
    }
}
