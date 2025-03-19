using System;
using UnityEngine;

public class PickUpState : IState
{
    private readonly CollectorAnimations _animations;
    
    private readonly Animator _animator;
    private readonly Transform _pickUpPoint;
    private readonly MoverToTarget _moverToTarget;

    public PickUpState(CollectorAnimations animations, Transform pickUpPoint, MoverToTarget moverToTarget)
    {
        _animations = animations ? animations : throw new NullReferenceException(nameof(animations));
        _pickUpPoint = pickUpPoint ? pickUpPoint : throw new NullReferenceException(nameof(pickUpPoint));
        _moverToTarget = moverToTarget ? moverToTarget : throw new NullReferenceException(nameof(moverToTarget));
    }
    
    public void Enter()
    {
        _moverToTarget.Target.SetParent(_pickUpPoint);
        _moverToTarget.Target.localPosition = Vector3.zero;
        _animations.PlayPickUp();
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
