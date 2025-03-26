using System;
using UnityEngine;

public class NearbyTransitionConditions : ITransitionCondition
{
    private readonly float _inaccuracy;
    private readonly Transform _self;
    
    private Transform _target;
    private Vector3 _offset;

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

    public NearbyTransitionConditions(Transform self, float inaccuracy)
    {
        _self = self ? self : throw new NullReferenceException(nameof(self));
        _inaccuracy = inaccuracy;
    }

    public NearbyTransitionConditions(Transform self, IPosition position, float inaccuracy)
    {
        _self = self ? self : throw new NullReferenceException(nameof(self));
        Target = position.Transform;
        _offset = position.Offset;
        _inaccuracy = inaccuracy;
    }
    
    public bool IsDone()
    {
        return Vector3.Distance(_self.position, _target.position + _target.rotation * _offset) < _inaccuracy;
    }
    
}
