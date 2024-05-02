using TMPro;
using UnityEngine;

public class JewelTest : MonoBehaviour
{
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private Sprite _finalSprite;
    [Space]
    [SerializeField] private TMP_Text _textCount;
    [SerializeField] private float _scaleFontSize = 2.5f;

    public void Setup(Vector3 position, Vector2 size, int count, bool isFinal)
    {
        transform.localPosition = position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = size;
        spriteRenderer.sprite = isFinal ? _finalSprite : _mainSprite;

        _textCount.GetComponent<RectTransform>().sizeDelta = size;
        _textCount.fontSize = size.x * _scaleFontSize;

        _textCount.text = count.ToString();
    }
}
