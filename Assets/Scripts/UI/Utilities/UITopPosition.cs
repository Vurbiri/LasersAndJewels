using UnityEngine;

[ExecuteInEditMode]
public class UITopPosition : MonoBehaviour
{
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _offset = -90f;

    private RectTransform _thisRectTransform;
    private float _heightOld = 0f;

    private void Awake()
    {
        _thisRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_heightOld == _canvas.sizeDelta.y)
            return;

        _heightOld = _canvas.sizeDelta.y;
        _thisRectTransform.localPosition = new(0, Mathf.Round(_heightOld / 100f) * 50f + _offset, 0);
    }
}
