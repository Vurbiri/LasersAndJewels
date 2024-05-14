using System;
using System.Collections;
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
    [Space]
    [SerializeField] private float _durationShowHide = 1f;

    private Color _colorOn, _colorOff;

    public Transform Transform => _spriteRenderer.gameObject.transform;

    public void Setup(Color color)
    {
        _spriteRenderer.color = color.SetAlpha(0f);
        _spriteRenderer.sprite = _spriteOff;

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

    public IEnumerator Appear() => SmoothAlpha_Coroutine(0f, _alfaOff);
    public IEnumerator Fade() => SmoothAlpha_Coroutine(_spriteRenderer.color.a, 0f);

    private IEnumerator SmoothAlpha_Coroutine(float start, float end)
    {
        float alpha, currentTime = 0f;
        Color color = _spriteRenderer.color;
        while (currentTime < _durationShowHide)
        {
            alpha = Mathf.Lerp(start, end, currentTime / _durationShowHide);
            _spriteRenderer.color = color.SetAlpha(alpha);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }
        _spriteRenderer.color = color.SetAlpha(end);
    }
}
