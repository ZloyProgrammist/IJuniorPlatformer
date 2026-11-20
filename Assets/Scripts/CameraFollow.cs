using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float _smoothTime = 0.2f;

    private Vector3 _velocity;

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (_target == null) 
            return;

        Vector3 desiredPosition = _target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothTime);
    }
}
