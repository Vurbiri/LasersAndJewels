using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AJewel : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    protected void Setup(Vector3 position, Vector2 size)
    {
        transform.localPosition = position;
        _spriteRenderer.size = size;
    }
}
