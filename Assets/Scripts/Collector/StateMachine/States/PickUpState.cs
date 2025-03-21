using System;
using UnityEngine;

public class PickUpState : IState
{
    private readonly Inventory _inventory;
    private readonly CollectorAnimations _animations;
    
    private readonly Animator _animator;
    private readonly Transform _pickUpPoint;
    private readonly MoverToTarget _moverToTarget;

    public PickUpState(CollectorAnimations animations, Transform pickUpPoint, MoverToTarget moverToTarget,
        Inventory inventory)
    {
        _animations = animations ? animations : throw new NullReferenceException(nameof(animations));
        _pickUpPoint = pickUpPoint ? pickUpPoint : throw new NullReferenceException(nameof(pickUpPoint));
        _moverToTarget = moverToTarget ? moverToTarget : throw new NullReferenceException(nameof(moverToTarget));
        _inventory = inventory ?? throw new NullReferenceException(nameof(inventory));
    }
    
    public void Enter()
    {
        _animations.PlayPickUp();
        
        var pickable = _moverToTarget.Target.GetComponent<IPickable>();
        pickable.PickUp(_pickUpPoint);
        
        _inventory.Put(pickable);
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
