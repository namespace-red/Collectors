using System;
using UnityEngine;

public class PositionPoint : IPosition
{
    private readonly Transform _transform;
    private readonly Vector3 _offset;

    public PositionPoint(Transform transform)
    {
        _transform = transform ? transform : throw new NullReferenceException(nameof(transform));
    }
    
    public PositionPoint(Transform transform, Vector3 offset)
    {
        _transform = transform;
        _offset = offset;
    }
    
    public Transform Transform => _transform;
    public Vector3 Offset => _offset;

    public Vector3 Get()
    {
        return Transform.position + Transform.rotation * Offset;
    }
}
