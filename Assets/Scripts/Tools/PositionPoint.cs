using System;
using UnityEngine;

public class PositionPoint : IPosition
{
    private Transform _transform;
    private Vector3 _offset;

    public PositionPoint()
    { }

    public PositionPoint(Transform transform)
    {
        Transform = transform ? transform : throw new ArgumentNullException(nameof(transform));
    }
    
    public PositionPoint(Transform transform, Vector3 offset)
    {
        Transform = transform ? transform : throw new ArgumentNullException(nameof(transform));
        Offset = offset;
    }
    
    public Transform Transform
    {
        get => _transform;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _transform = value;
        }
    }

    public Vector3 Offset
    {
        get => _offset;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _offset = value;
        }
    }

    public Vector3 Get()
    {
        return Transform.position + Transform.rotation * Offset;
    }
}
