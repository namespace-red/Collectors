public class ColonyCreaterState : IState
{
    private const int ColonyPrice = 5;

    private readonly ResourceWarehouse _resourceWarehouse;
    // private readonly ColonyFactory _colonyFactory;
    //
    // public ColonyCreaterState(ResourceWarehouse resourceWarehouse, ColonyFactory colonyFactory)
    // {
    //     _resourceWarehouse = resourceWarehouse ? resourceWarehouse : throw new ArgumentNullException(nameof(resourceWarehouse));
    //     _colonyFactory = colonyFactory ? colonyFactory : throw new ArgumentNullException(nameof(colonyFactory));
    // }

    public void Enter()
    {
        _resourceWarehouse.ChangedCount += OnChangedCountResource;
        CreateColony();
    }

    public void Exit()
    {
        _resourceWarehouse.ChangedCount -= OnChangedCountResource;
    }

    public void FixedUpdate() { }

    public void Update() { }
    
    private void OnChangedCountResource(int _)
        => CreateColony();

    private void CreateColony()
    {
        if (_resourceWarehouse.IsEnough(ColonyPrice))
        {
            _resourceWarehouse.Spend(ColonyPrice);
            // var colony = _colonyFactory.Create();
        }
    }
}
