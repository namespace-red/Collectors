using System;
using UnityEngine;

public class PositionInBox : IPosition
{
    private Transform _transform;
    private Vector3 _offset;

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

    public void SetRandomOffset(Bounds bounds)
    {
        PointInBox pointInBox = new PointInBox(bounds);
        _offset = pointInBox.GetRandom() - _transform.position;
    }
}
