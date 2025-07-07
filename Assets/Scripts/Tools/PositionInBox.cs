using System;
using UnityEngine;

public class PositionInBox : IPosition
{
    private readonly Transform _transform;
    private readonly Vector3 _offset;

    public PositionInBox(Transform transform, Bounds bounds)
    {
        _transform = transform ? transform : throw new ArgumentNullException(nameof(transform));

        PointInBox pointInBox = new PointInBox(bounds);
        _offset = pointInBox.GetRandom() - _transform.position;
    }
    
    public Transform Transform
    {
        get => _transform;
        set => throw new NotSupportedException();
    }

    public Vector3 Offset
    {
        get => _offset;
        set => throw new NotSupportedException();
    }

    public Vector3 Get()
    {
        return Transform.position + Transform.rotation * Offset;
    }
}
