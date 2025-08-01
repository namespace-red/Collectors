using System;
using UnityEngine;

public class ColonyCreaterState : IState
{
    private const int ColonyPrice = 5;

    private readonly ColonyFactory _colonyFactory;
    private readonly Colony _colony;

    public ColonyCreaterState(ColonyFactory colonyFactory, Colony colony)
    {
        _colonyFactory = colonyFactory ? colonyFactory : throw new ArgumentNullException(nameof(colonyFactory));
        _colony = colony ? colony : throw new ArgumentNullException(nameof(colony));
    }

    public void Enter()
    {
        _colony.ResourceWarehouse.ChangedCount += OnChangedCountResource;
        TrySendCollectorToFlag();
    }

    public void Exit()
    {
    }

    public void FixedUpdate() { }

    public void Update()
    {
        if (_colony.NeedSendCollectorForPickable || _colony.HaveFreeCollector() == false)
            return;

        _colony.ResourceWarehouse.ChangedCount -= OnChangedCountResource;
        var collector = _colony.GetFreeCollector();
        collector.GotToFlag += CreateColony;

        if (collector.TryGoToFlag())
        {
            _colony.NeedSendCollectorForPickable = true;
        }
    }
    
    private void OnChangedCountResource(int _)
        => TrySendCollectorToFlag();

    private void TrySendCollectorToFlag()
    {
        if (_colony.ResourceWarehouse.IsEnough(ColonyPrice))
        {
            _colony.NeedSendCollectorForPickable = false;
        }
    }

    private void CreateColony(Collector collector)
    {
        collector.GotToFlag -= CreateColony;
        _colony.ResourceWarehouse.Spend(ColonyPrice);
        _colony.Flag.Remove();
        _colony.RemoveCollector(collector);

        var newColony = _colonyFactory.Create(_colony.Flag.transform.position);
        newColony.AddCollector(collector);
    }
}
