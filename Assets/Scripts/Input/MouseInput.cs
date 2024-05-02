using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private float _distance = 300;
    [SerializeField] private Vector3 _direction = Vector3.back;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Click(true);
        else if (Input.GetMouseButtonDown(1))
            Click(false);

        #region Local function
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
