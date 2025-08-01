using System;

public class CollectorCreaterState : IState
{
    private readonly CollectorFactory _collectorFactory;
    private readonly ResourceWarehouse _resourceWarehouse;
    private readonly int _collectorPrice;

    public CollectorCreaterState(CollectorFactory collectorFactory, int startCollectorCount, 
        ResourceWarehouse resourceWarehouse, int collectorPrice)
    {
        _collectorFactory = collectorFactory ? collectorFactory : throw new ArgumentNullException(nameof(collectorFactory));
        _resourceWarehouse = resourceWarehouse ? resourceWarehouse : throw new ArgumentNullException(nameof(resourceWarehouse));
        _collectorPrice = collectorPrice;

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
        if (_resourceWarehouse.IsEnough(_collectorPrice))
        {
            int count = _resourceWarehouse.Count / _collectorPrice;
            _resourceWarehouse.Spend(count * _collectorPrice);
            _collectorFactory.Create(count);
        }
    }
}
