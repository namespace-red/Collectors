using System;
using UnityEngine;

namespace CollectorStateMachine
{
    public class PickUpState : IState
    {
        private const int AnimationLayer = 0;
    
        private readonly int _animationHash = Animator.StringToHash("Base Layer.PickingUp1");
        private readonly Collector _collector;
        private readonly Transform _pickUpPoint;

        public PickUpState(Collector collector, Transform pickUpPoint)
        {
            _collector = collector ? collector : throw new ArgumentNullException(nameof(collector));
            _pickUpPoint = pickUpPoint ? pickUpPoint : throw new ArgumentNullException(nameof(pickUpPoint));
        }
    
        public void Enter()
        {
            _collector.Animations.PlayPickUp1();
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
                IPickable pickable = _collector.PickablePosition.Transform.GetComponent<IPickable>();
                pickable.PickUp(_pickUpPoint);
                _collector.Inventory.Put(pickable);
            
                _collector.Animations.PlayPickUp2();
            }
        }
    }
}
