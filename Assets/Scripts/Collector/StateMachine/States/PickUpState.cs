using System;
using UnityEngine;

public class PickUpState : IState
{
    private const int AnimationLayer = 0;
    private readonly int _animationHash = Animator.StringToHash("Base Layer.PickingUp1");
    
    private readonly CollectorAnimations _animations;
    private readonly Transform _pickUpPoint;
    private readonly MoverToTarget _moverToTarget;
    private readonly Inventory _inventory;

    public PickUpState(CollectorAnimations animations, Transform pickUpPoint, MoverToTarget moverToTarget,
        Inventory inventory)
    {
        _animations = animations ? animations : throw new ArgumentNullException(nameof(animations));
        _pickUpPoint = pickUpPoint ? pickUpPoint : throw new ArgumentNullException(nameof(pickUpPoint));
        _moverToTarget = moverToTarget ? moverToTarget : throw new ArgumentNullException(nameof(moverToTarget));
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
    }
    
    public void Enter()
    {
        _animations.PlayPickUp1();
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        AnimatorStateInfo animatorStateInfo = _animations.Animator.GetCurrentAnimatorStateInfo(AnimationLayer);

        if (animatorStateInfo.fullPathHash == _animationHash && animatorStateInfo.normalizedTime >= 1 &&
            _animations.Animator.IsInTransition(AnimationLayer) == false)
        {
            var pickable = _moverToTarget.Target.GetComponent<IPickable>();
            pickable.PickUp(_pickUpPoint);
            _inventory.Put(pickable);
            
            _animations.PlayPickUp2();
        }
    }
}
