using UnityEngine;

public class CollectorSpawner : Spawner<Collector>
{
    [SerializeField] private PositionInArea _waitArea;

    public override Collector Get()
    {
        var collector = base.Get();
        collector.MoverToWaitPoint.TargetPoint = _waitArea.Get();
        return collector;
    }
}
