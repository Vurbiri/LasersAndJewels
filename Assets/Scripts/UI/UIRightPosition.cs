using UnityEngine;

public class UIRightPosition : MonoBehaviour
{
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _offset = -90f;

    private RectTransform _thisRectTransform;
    private float _widthOld = 0f;

    private void Start()
    {
        _thisRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_widthOld == _canvas.sizeDelta.x)
            return;

        _widthOld = _canvas.sizeDelta.x;
        _thisRectTransform.localPosition = new(Mathf.Round(_widthOld / 100f) * 50f + _offset, 0, 0);
    }
}
