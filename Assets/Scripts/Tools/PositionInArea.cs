using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PositionInArea : IPosition
{
    private readonly Transform _transform;
    private readonly Vector3 _offset;

    public PositionInArea(Transform transform, Bounds bounds)
    {
        _transform = transform ? transform : throw new ArgumentNullException(nameof(transform));

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.center.y;
        _offset = new Vector3(x, y, z) - _transform.position;
    }
    
    public Transform Transform
    {
        get => _transform;
        set => throw new NotImplementedException();
    }

    public Vector3 Offset
    {
        get => _offset;
        set => throw new NotImplementedException();
    }

    public Vector3 Get()
    {
        return Transform.position + Transform.rotation * Offset;
    }
}
