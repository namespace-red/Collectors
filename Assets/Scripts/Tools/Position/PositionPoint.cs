using System;
using UnityEngine;

public class PositionPoint : IPosition
{
    private Transform _transform;
    private Vector3 _offset;

    public Transform Transform
    {
        get => _transform;
        set => _transform = value;
    }

    public Vector3 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    public Vector3 Get()
    {
        return Transform.position + Transform.rotation * Offset;
    }
}
