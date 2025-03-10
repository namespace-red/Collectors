using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoverToPoint : BasePhysicsMover
{
    private Vector3 _targetPoint;

    public Vector3 TargetPoint
    {
        get => _targetPoint;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _targetPoint = value;
        }
    }

    protected override Vector3 Direction => (_targetPoint - transform.position).normalized;
}
