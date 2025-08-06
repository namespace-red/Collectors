using System;

namespace CollectorStateMachine
{
    public class MoverToTargetState : IState
    {
        private readonly Collector _collector;
        private readonly IPosition _target;

        public MoverToTargetState(Collector collector, IPosition target)
        {
            _collector = collector ? collector : throw new ArgumentNullException(nameof(collector));
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }
   
        public void Enter()
        {
            _collector.Animations.PlayRun();
        
            _collector.MoverToTarget.Target = _target.Transform;
            _collector.MoverToTarget.Offset = _target.Offset;
            _collector.MoverToTarget.enabled = true;
        }

        public void Exit()
        {
            _collector.MoverToTarget.enabled = false;
        }
    
        public void FixedUpdate()
        {
        }

        public void Update()
        {
        }
    }
}
