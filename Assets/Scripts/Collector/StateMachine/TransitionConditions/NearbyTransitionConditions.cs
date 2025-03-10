using UnityEngine;

public class NearbyTransitionConditions : ITransitionCondition
{
    private const float Inaccuracy = 0.1f;
    
    private readonly Transform _self;
    private readonly Vector3 _targetPoint;

    public NearbyTransitionConditions(Transform self, Vector3 targetPoint)
    {
        _self = self;
        _targetPoint = targetPoint;
    }
    
    public bool IsDone()
    {
        return Vector3.Distance(_self.position, _targetPoint) < Inaccuracy;
    }
}
