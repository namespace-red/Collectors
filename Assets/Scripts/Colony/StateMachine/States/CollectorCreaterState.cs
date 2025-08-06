using System;

namespace ColonyStateMachine
{
    public class CollectorCreaterState : IState
    {
        private readonly Colony _colony;
        private readonly int _collectorPrice;

        public CollectorCreaterState(Colony colony, int startCollectorCount, int collectorPrice)
        {
            _colony = colony ? colony : throw new ArgumentNullException(nameof(colony));
            _collectorPrice = collectorPrice;

            _colony.CollectorFactory.Create(startCollectorCount);
        }

        public void Enter()
        {
            _colony.ResourceWarehouse.ChangedCount += OnChangedCountResource;
            TryCreateCollector();
        }

        public void Exit()
        {
            _colony.ResourceWarehouse.ChangedCount -= OnChangedCountResource;
        }

        public void FixedUpdate() { }

        public void Update() { }
    
        private void OnChangedCountResource(int _)
            => TryCreateCollector();
    
        private void TryCreateCollector()
        {
            if (_colony.ResourceWarehouse.IsEnough(_collectorPrice))
            {
                int count = _colony.ResourceWarehouse.Count / _collectorPrice;
                _colony.ResourceWarehouse.Spend(count * _collectorPrice);
                _colony.CollectorFactory.Create(count);
            }
        }
    }
}
