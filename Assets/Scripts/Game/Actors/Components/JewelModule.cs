using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class JewelModule : MonoBehaviour
{
    [SerializeField] protected SpriteModule _spriteModule;

    private Transform _thisTransform;

    private int _idType;
    private bool _isOn = true, _isVisited = false;
    private Quaternion _rotationDefault;

    public int IdType => _idType;
    public bool IsVisited { get => _isVisited; set => _isVisited = value; }

    public void Initialize()
    {
        _thisTransform = transform;
        _rotationDefault = transform.rotation;
        _spriteModule.Initialize();
    }

    public void Setup(int idType, Color color) => Setup(idType, color, _rotationDefault);

    public void Setup(int idType, Color color, Vector2Int orientation) => Setup(idType, color, TurnData.TurnFromOrientation(orientation));

    public void Setup(int idType, Color color, Quaternion turn)
    {
        _idType = idType;

        _thisTransform.localRotation = turn;

        _spriteModule.Setup(color);

        _isOn = true;
        _isVisited = false;

        Off();
    }

    public void Switch()
    {
        if (_isVisited)
            On();
        else
            Off();

        _isVisited = false;
    }

    public void On()
    {
        if (_isOn) return;

        _isOn = true;
        _spriteModule.On();
    }

    public void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _spriteModule.Off();
    }
}


