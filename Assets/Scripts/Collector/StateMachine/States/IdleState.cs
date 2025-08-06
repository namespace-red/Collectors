namespace CollectorStateMachine
{
    public class IdleState : IState
    {
        private readonly CollectorAnimations _animations;
    
        public IdleState(CollectorAnimations animations)
        {
            _animations = animations;
        }
        
        public void Enter()
        {
            _animations.PlayIdle();
        }
    
        public void Exit()
        {
        }
    
        public void FixedUpdate()
        {
        }
    
        public void Update()
        {
        }
    }
}

