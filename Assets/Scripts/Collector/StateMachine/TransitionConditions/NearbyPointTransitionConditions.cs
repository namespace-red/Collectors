using System;
using UnityEngine;

public class NearbyPointTransitionConditions : ITransitionCondition
{
    private const float Inaccuracy = 0.1f;
    
    private readonly Transform _self;
    private readonly Vector3 _targetPoint;

    public NearbyPointTransitionConditions(Transform self, Vector3 targetPoint)
    {
        _self = self ? self : throw new NullReferenceException(nameof(self));
        _targetPoint = targetPoint;
    }
    
    public bool IsDone()
    {
        return Vector3.Distance(_self.position, _targetPoint) < Inaccuracy;
    }
}
