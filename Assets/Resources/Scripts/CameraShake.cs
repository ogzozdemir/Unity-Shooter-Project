using System;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform shakeCamera;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;
    [SerializeField] private float duration;
    
    private static event Action Shake;
    
    public static void Invoke() => Shake?.Invoke();
    
    private void OnEnable() => Shake += ShakeMethod;
    private void OnDisable() => Shake -= ShakeMethod;

    private void ShakeMethod()
    {
        shakeCamera.DOComplete();
        shakeCamera.DOShakePosition(duration, positionStrength);
        shakeCamera.DOShakeRotation(duration, rotationStrength);
    }
}
