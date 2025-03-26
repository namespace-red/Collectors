using System;
using UnityEngine;

public class MoverToTarget : BasePhysicsMover
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    
    public Transform Target
    {
        get => _target;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _target = value;
        }
    }
    
    public Vector3 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    protected override Vector3 Direction => (_target.position + _target.rotation * Offset - transform.position).normalized;
}
