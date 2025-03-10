using System;
using UnityEngine;

public class MoverToTarget : BasePhysicsMover
{
    [SerializeField] private Transform _target;
    
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

    protected override Vector3 Direction => (_target.position - transform.position).normalized;
}
