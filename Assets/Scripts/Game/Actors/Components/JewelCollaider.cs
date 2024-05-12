using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class JewelCollider : MonoBehaviour, IMouseClick
{
    private Transform _thisTransform;
    private Collider2D _collider;

    public bool IsInteractable { set => _collider.enabled = value; }
    public Quaternion Rotation { set => _thisTransform.localRotation = value; }
    public Vector3 LocalPosition { get => _thisTransform.localPosition;  set => _thisTransform.localPosition = value; }

    public event Action<bool> EventClick;

    public void Initialize()
    {
        _thisTransform = transform;
        _collider = GetComponent<Collider2D>();

        _collider.enabled = false;
    }

    public void OnMouseClick(bool isLeft) => EventClick?.Invoke(isLeft);
}
