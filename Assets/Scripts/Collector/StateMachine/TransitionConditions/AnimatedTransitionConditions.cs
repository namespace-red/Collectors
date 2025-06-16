using System;
using UnityEngine;

public class AnimatedTransitionConditions : ITransitionCondition
{
    private readonly Animator _animator;
    private readonly int _layer;
    private readonly string _name;

    public AnimatedTransitionConditions(Animator animator, int layer, string name)
    {
        _animator = animator ? animator : throw new ArgumentNullException(nameof(animator));
        _name = name;
        _layer = layer;
    }
    
    public bool IsDone()
    {
        AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(_layer);
        
        bool result = animatorStateInfo.IsName(_name) && animatorStateInfo.normalizedTime >= 1 && 
                      _animator.IsInTransition(_layer) == false;
        
        return result;
    }
}
