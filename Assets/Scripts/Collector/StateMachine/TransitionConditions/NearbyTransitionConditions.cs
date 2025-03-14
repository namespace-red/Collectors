using System;
using UnityEngine;

public class NearbyTransitionConditions : ITransitionCondition
{
    private const float Inaccuracy = 1.1f;
    
    private readonly Transform _self;
    private Transform _target;

    public Transform Target
    {
        get => _target;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(Target));

            _target = value;
        }
    }

    public NearbyTransitionConditions(Transform self)
    {
        _self = self;
    }
    
    public bool IsDone()
    {
        return Vector3.Distance(_self.position, _target.position) < Inaccuracy;
    }
    
}
