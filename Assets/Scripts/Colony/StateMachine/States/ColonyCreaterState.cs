using System;

public class ColonyCreaterState : IState
{
    private readonly ColonyFactory _colonyFactory;
    private readonly Colony _colony;
    private readonly int _colonyPrice;

    public ColonyCreaterState(ColonyFactory colonyFactory, Colony colony, int colonyPrice)
    {
        _colonyFactory = colonyFactory ? colonyFactory : throw new ArgumentNullException(nameof(colonyFactory));
        _colony = colony ? colony : throw new ArgumentNullException(nameof(colony));
        _colonyPrice = colonyPrice;
    }

    public void Enter()
    {
        _colony.ResourceWarehouse.ChangedCount += OnChangedCountResource;
        TrySendCollectorToFlag();
    }

    public void Exit() { }

    public void FixedUpdate() { }

    public void Update()
    {
        if (_colony.NeedSendCollectorForPickable || _colony.HaveFreeCollector() == false)
            return;

        _colony.ResourceWarehouse.ChangedCount -= OnChangedCountResource;
        _colony.NeedSendCollectorForPickable = true;
        
        var collector = _colony.GetFreeCollector();
        collector.GotToFlag += CreateColony;
        collector.GoToFlag();
    }
    
    private void OnChangedCountResource(int _)
        => TrySendCollectorToFlag();

    private void TrySendCollectorToFlag()
    {
        if (_colony.ResourceWarehouse.IsEnough(_colonyPrice))
        {
            _colony.NeedSendCollectorForPickable = false;
        }
    }

    private void CreateColony(Collector collector)
    {
        collector.GotToFlag -= CreateColony;
        _colony.ResourceWarehouse.Spend(_colonyPrice);
        _colony.Flag.Remove();
        _colony.RemoveCollector(collector);

        var newColony = _colonyFactory.Create(_colony.Flag.transform.position);
        newColony.AddCollector(collector);
    }
}
