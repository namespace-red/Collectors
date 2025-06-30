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
        get
        {
            Debug.Log("Transform get " + _transform);
            return _transform;
        }
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            Debug.Log("Transform set " + value);
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
        Debug.Log("Get " + Transform.name);
        return Transform.position + Transform.rotation * Offset;
    }
}
