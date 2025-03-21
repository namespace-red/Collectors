using System;
using UnityEngine;

public class NearbyPointTransitionConditions : ITransitionCondition
{
    private readonly float _inaccuracy;
    private readonly Transform _self;
    private readonly Vector3 _targetPoint;

    public NearbyPointTransitionConditions(Transform self, Vector3 targetPoint, float inaccuracy)
    {
        _self = self ? self : throw new NullReferenceException(nameof(self));
        _targetPoint = targetPoint;
        _inaccuracy = inaccuracy;
    }
    
    public bool IsDone()
    {
        return Vector3.Distance(_self.position, _targetPoint) < _inaccuracy;
    }
}
