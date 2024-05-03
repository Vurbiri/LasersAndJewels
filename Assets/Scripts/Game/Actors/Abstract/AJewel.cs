using UnityEngine;

public abstract class AJewel<T> : APooledObject<T>, IJewel where T : AJewel<T>
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Color _colorOn = Color.white;
    [SerializeField] protected Color _colorOff = Color.gray;

    public Vector2Int Index { get; protected set; }
    public Vector2Int Orientation { get; protected set; } = Vector2Int.zero;
    public Vector3 LocalPosition => _thisTransform.localPosition;

    public static float Size
    {
        set
        {
            s_size = value;
            s_startPos = value * 0.5f;
        }
    }
    protected static float s_size, s_startPos;

    public static Vector3 CalkLocalPosition(Vector2Int index) => new(s_startPos + index.x * s_size, s_startPos + index.y * s_size, 0f);

    public override void Initialize()
    {
        _spriteRenderer.size = Vector2.one * s_size;

       base.Initialize();
    }

    protected void BaseSetup(Vector2Int index)
    {
        Index = index;
        transform.localPosition = CalkLocalPosition(index);

        Activate();
    }

    public void On()
    {
        _spriteRenderer.color = _colorOn;
    }

    public void Off()
    {
        _spriteRenderer.color = _colorOff;
    }
}
