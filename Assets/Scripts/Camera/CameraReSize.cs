using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraReSize : MonoBehaviour
{
    [SerializeField] private CanvasScaler _canvasScaler;

    private Camera _thisCamera;
    private float _aspectRatioOld = 0f;
    private float _verticalHalfSizeMin = 5f;
    private float _horizontalHalfSizeMin = 8f;

    private void Awake()
    {
        _thisCamera = GetComponent<Camera>();
        _verticalHalfSizeMin = _canvasScaler.referenceResolution.y / 200f;
        _horizontalHalfSizeMin = _canvasScaler.referenceResolution.x / 200f;
    }

    private void Update()
    {
        if (_aspectRatioOld == _thisCamera.aspect)
            return;

        _aspectRatioOld = _thisCamera.aspect;
        float horizontalHalfSize = _verticalHalfSizeMin * _aspectRatioOld;

        if (horizontalHalfSize < _horizontalHalfSizeMin)
            _thisCamera.orthographicSize = _horizontalHalfSizeMin / _aspectRatioOld;
        else
            _thisCamera.orthographicSize = _verticalHalfSizeMin;
    }
}
