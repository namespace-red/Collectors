using System;

public class CollectorCreaterState : IState
{
    private const int CollectorPrice = 3;

    private readonly ResourceWarehouse _resourceWarehouse;
    private readonly CollectorFactory _collectorFactory;
    
    public CollectorCreaterState(ResourceWarehouse resourceWarehouse, CollectorFactory collectorFactory, int startCollectorCount)
    {
        _resourceWarehouse = resourceWarehouse ? resourceWarehouse : throw new ArgumentNullException(nameof(resourceWarehouse));
        _collectorFactory = collectorFactory ? collectorFactory : throw new ArgumentNullException(nameof(collectorFactory));
        
        _collectorFactory.Create(startCollectorCount);
    }

    public void Enter()
    {
        _resourceWarehouse.ChangedCount += OnChangedCountResource;
        CreateCollector();
    }

    public void Exit()
    {
        _resourceWarehouse.ChangedCount -= OnChangedCountResource;
    }

    public void FixedUpdate() { }

    public void Update() { }

    private void OnChangedCountResource(int _)
        => CreateCollector();
    
    private void CreateCollector()
    {
        if (_resourceWarehouse.IsEnough(CollectorPrice))
        {
            int count = _resourceWarehouse.Count / CollectorPrice;
            _resourceWarehouse.Spend(count * CollectorPrice);
            _collectorFactory.Create(count);
        }
    }
}
