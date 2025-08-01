using System;

public class CollectorCreaterState : IState
{
    private const int CollectorPrice = 3;
    
    private readonly CollectorFactory _collectorFactory;
    private readonly ResourceWarehouse _resourceWarehouse;
    
    public CollectorCreaterState(CollectorFactory collectorFactory, int startCollectorCount, ResourceWarehouse resourceWarehouse)
    {
        _collectorFactory = collectorFactory ? collectorFactory : throw new ArgumentNullException(nameof(collectorFactory));
        _resourceWarehouse = resourceWarehouse ? resourceWarehouse : throw new ArgumentNullException(nameof(resourceWarehouse));
    
        _collectorFactory.Create(startCollectorCount);
    }

    public void Enter()
    {
        _resourceWarehouse.ChangedCount += OnChangedCountResource;
        TryCreateCollector();
    }

    public void Exit()
    {
        _resourceWarehouse.ChangedCount -= OnChangedCountResource;
    }

    public void FixedUpdate() { }

    public void Update() { }
    
    private void OnChangedCountResource(int _)
        => TryCreateCollector();
    
    private void TryCreateCollector()
    {
        if (_resourceWarehouse.IsEnough(CollectorPrice))
        {
            int count = _resourceWarehouse.Count / CollectorPrice;
            _resourceWarehouse.Spend(count * CollectorPrice);
            _collectorFactory.Create(count);
        }
    }
}
