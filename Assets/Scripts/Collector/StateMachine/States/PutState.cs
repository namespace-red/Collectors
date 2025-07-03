using System;
using UnityEngine;

public class PutState : IState
{
    private const int AnimationLayer = 0;
    private const string AnimationName = "Put1";

    private readonly CollectorAnimations _animations;
    private readonly Inventory _inventory;
    
    public event Action<IPickable> PutPickable;

    public PutState(CollectorAnimations animations, Inventory inventory)
    {
        _animations = animations ? animations : throw new ArgumentNullException(nameof(animations));
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
    }
    
    public void Enter()
    {
        _animations.PlayPut1();
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

        if (animatorStateInfo.IsName(AnimationName) && animatorStateInfo.normalizedTime >= 1 &&
            _animations.Animator.IsInTransition(AnimationLayer) == false)
        {
            var pickable = _inventory.Take();
            pickable.Put();
            _animations.PlayPut2();
            
            PutPickable?.Invoke(pickable);
        }
    }
}
