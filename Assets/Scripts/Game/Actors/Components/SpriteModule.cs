using System;
using UnityEngine;

[Serializable]
public class SpriteModule
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [Space]
    [SerializeField] private float _alfaOn = 0.85f;
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private float _alfaOff = 0.55f;
    [SerializeField] private Sprite _spriteOff;

    private Color _colorOn, _colorOff;
    public Transform Transform => _spriteRenderer.gameObject.transform;

    public void Setup(Color color)
    {
        _colorOn = color.SetAlpha(_alfaOn);
        _colorOff = color.SetAlpha(_alfaOff);
    }

    public void On()
    {
        _spriteRenderer.color = _colorOn;
        _spriteRenderer.sprite = _spriteOn;
    }

    public void Off()
    {
        _spriteRenderer.color = _colorOff;
        _spriteRenderer.sprite = _spriteOff;
    }
}
