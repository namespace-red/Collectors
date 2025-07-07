using System;
using UnityEngine;

public class NearbyTransitionConditions : ITransitionCondition
{
    private readonly float _inaccuracy;
    private readonly Transform _self;
    private readonly IPosition _target;

    public NearbyTransitionConditions(Transform self, IPosition target, float inaccuracy)
    {
        _self = self ? self : throw new ArgumentNullException(nameof(self));
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _inaccuracy = inaccuracy;
    }

    public bool IsDone()
    {
        return _self.position.IsEnoughClose(_target.Get(), _inaccuracy);
    }
}
