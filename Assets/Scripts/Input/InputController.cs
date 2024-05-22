using System;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private Button _buttonMenu;
    [SerializeField] private KeyCode _keyMenu = KeyCode.Tab;
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonRestart;
    [Space]
    [SerializeField] private Button _buttonHint;
    [SerializeField] private KeyCode _keyHint = KeyCode.H;
    [Space]
    [SerializeField] private float _distance = 300;
    [SerializeField] private Vector3 _direction = Vector3.back;

    private Camera _camera;

    private bool _isMenu = false;
    
    public bool IsMenu
    {
        get => _isMenu;
        set
        {
            if (_isMenu == value)
                return;
            
            _isMenu = value;
            if (value)
                EventToMenu?.Invoke();
            else
                EventContinue?.Invoke();
        }
    }

    public event Action EventToMenu;
    public event Action EventContinue;
    public event Action EventRestart;
    public event Action EventHint;

    private void Awake()
    {
        _camera = Camera.main;

        _buttonMenu.onClick.AddListener(OnButtonMenu);
        _buttonStart.onClick.AddListener(() => IsMenu = false);
        _buttonRestart.onClick.AddListener(() => { EventRestart?.Invoke(); _isMenu = false; });
        _buttonHint.onClick.AddListener(() => { if (!_isMenu) EventHint?.Invoke(); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keyMenu))
            OnButtonMenu();

        else if (_isMenu) return;

        else if (Input.GetKeyDown(_keyHint))
            EventHint?.Invoke();

        else if (Input.GetMouseButtonDown(0))
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

    private void OnButtonMenu() => IsMenu = !_isMenu;
}
