using System;

namespace CollectorStateMachine
{
    public class MoverToFlagState : IState
    {
        private readonly Collector _collector;
        private readonly IPosition _flagPosition;

        public MoverToFlagState(Collector collector, IPosition flagPosition)
        {
            _collector = collector ? collector : throw new ArgumentNullException(nameof(collector));
            _flagPosition = flagPosition ?? throw new ArgumentNullException(nameof(flagPosition));
        }
   
        public void Enter()
        {
            _collector.Animations.PlayRun();
        
            _collector.MoverToTarget.Target = _flagPosition.Transform;
            _collector.MoverToTarget.Offset = _flagPosition.Offset;
            _collector.MoverToTarget.enabled = true;
        }

        public void Exit()
        {
            _collector.MoverToTarget.enabled = false;
            _collector.CompleteGoToFlag();
        }
    
        public void FixedUpdate()
        {
        }

        public void Update()
        {
        }
    }
}