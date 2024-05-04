using UnityEngine;

public abstract class AJewel<T> : APooledObject<T>, IJewel where T : AJewel<T>
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected ParticleSystem _particle;
    [Space]
    [SerializeField] protected Color _colorOn = Color.white;
    [SerializeField] protected Sprite _spriteOn;
    [SerializeField] protected Color _colorOff = Color.gray;
    [SerializeField] protected Sprite _spriteOff;

    public Vector2Int Index { get; protected set; }
    public Vector2Int Orientation { get; protected set; } = Vector2Int.zero;
    public Vector3 LocalPosition => _thisTransform.localPosition;


    //protected const float startPos = 0.5f;

    //public static Vector3 CalkLocalPosition(Vector2Int index) => new(startPos + index.x, startPos + index.y, 0f);

    //public override void Initialize()
    //{
    //    _spriteRenderer.size = Vector2.one * s_size;

    //    //ParticleSystem.MainModule main = _particle.main;
    //    //main.startColor = _colorOn;

    //   base.Initialize();
    //}

    protected void BaseSetup(Vector2Int index)
    {
        Index = index;
        transform.localPosition = index.ToVector3();

        Activate();
    }

    public void On()
    {
        _spriteRenderer.color = _colorOn;
        _spriteRenderer.sprite = _spriteOn;
        _particle.Play();
    }

    public void Off()
    {
        _spriteRenderer.color = _colorOff;
        _spriteRenderer.sprite = _spriteOff;
        _particle.Stop();
    }
}
