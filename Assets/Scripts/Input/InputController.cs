using System;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private Button _buttonHint;
    [SerializeField] private KeyCode _keyHint = KeyCode.H;
    [Space]
    [SerializeField] private float _distance = 300;
    [SerializeField] private Vector3 _direction = Vector3.back;

    private Camera _camera;

    private bool _isHint = false;
    
    public bool IsHint
    {
        set
        {
            _isHint = value;
            _buttonHint.interactable = value;
        }
    }


    public event Action EventHint;

    private void Awake()
    {
        _camera = Camera.main;

        _buttonHint.onClick.AddListener(() => { if (_isHint) EventHint?.Invoke(); });
        IsHint = false;

    }

    private void Update()
    {

        if (_isHint && Input.GetKeyDown(_keyHint))
            EventHint?.Invoke();


        if (Input.GetMouseButtonDown(0))
            Click(true);
        else if (Input.GetMouseButtonDown(1))
            Click(false);

        


        #region Local: Click(...)
        //======================
        void Click(bool isLeft)
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _direction, _distance);
            if (hit.collider != null && hit.collider.TryGetComponent(out IMouseClick mouseClick))
                mouseClick.OnMouseClick(isLeft);
        }
        #endregion
    }
}
