using System;
using UnityEngine;

namespace CollectorStateMachine
{
    public class PutState : IState
    {
        private const int AnimationLayer = 0;
    
        private readonly int _animationHash = Animator.StringToHash("Base Layer.Put1");
        private readonly Collector _collector;
    
        public PutState(Collector collector)
        {
            _collector = collector ? collector : throw new ArgumentNullException(nameof(collector));
        }
    
        public void Enter()
        {
            _collector.Animations.PlayPut1();
        }

        public void Exit()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Update()
        {
            AnimatorStateInfo animatorStateInfo = _collector.Animations.Animator.GetCurrentAnimatorStateInfo(AnimationLayer);

            if (animatorStateInfo.fullPathHash == _animationHash && animatorStateInfo.normalizedTime >= 1 &&
                _collector.Animations.Animator.IsInTransition(AnimationLayer) == false)
            {
                var pickable = _collector.Inventory.Take();
                pickable.Put();
                _collector.Animations.PlayPut2();
                _collector.CompletePutPickable(pickable);
            }
        }
    }
}
