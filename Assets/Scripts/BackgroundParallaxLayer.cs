using UnityEngine;

public class BackgroundParallaxLayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;

    [Header("Parallax Settings")]
    [Range(0f, 1f)] [SerializeField] private float _parallaxFactor = 0.5f;

    private Vector3 _startPosition;
    private Vector3 _startCameraPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _startCameraPosition = _camera.position;
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {
        if (_camera == null) 
            return;

        Vector3 cameraDelta = _camera.position - _startCameraPosition;

        transform.position = _startPosition + new Vector3(cameraDelta.x * _parallaxFactor, cameraDelta.y * _parallaxFactor, 0f);
    }
}