using System;
using UnityEngine;

[RequireComponent(typeof(ResourceWarehouse))]
public class WarehouseHandler : MonoBehaviour
{
    private const int CollectorPrice = 3;

    [SerializeField] private CollectorFactory collectorFactory;
    [SerializeField, Min(0)] private int _startCollectorCount;
    
    private ResourceWarehouse _resourceWarehouse;

    public event Action<Collector> Created;

    private void OnValidate()
    {
        if (collectorFactory == null)
            throw new NullReferenceException(nameof(collectorFactory));
    }

    private void Awake()
    {
        _resourceWarehouse = GetComponent<ResourceWarehouse>();
    }

    private void OnEnable()
    {
        _resourceWarehouse.ChangedCount += OnChangedCountResource;
        collectorFactory.Created += OnCreated;
    }

    private void OnDisable()
    {
        _resourceWarehouse.ChangedCount -= OnChangedCountResource;
        collectorFactory.Created -= OnCreated;
    }

    private void Start()
    {
        collectorFactory.Create(_startCollectorCount);
    }

    private void OnChangedCountResource(int obj)
    {
        if (_resourceWarehouse.IsEnough(CollectorPrice))
        {
            int count = _resourceWarehouse.Count / CollectorPrice;
            _resourceWarehouse.Spend(count * CollectorPrice);
            collectorFactory.Create(count);
        }
    }

    private void OnCreated(Collector collector)
    {
        Created?.Invoke(collector);
    }
}
