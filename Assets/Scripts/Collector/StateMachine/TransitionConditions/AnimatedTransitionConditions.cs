using System;
using UnityEngine;

public class AnimatedTransitionConditions : ITransitionCondition
{
    private readonly Animator _animator;
    private readonly int _layer;
    private readonly int _hash;

    public AnimatedTransitionConditions(Animator animator, int layer, int hash)
    {
        _animator = animator ? animator : throw new ArgumentNullException(nameof(animator));
        _hash = hash;
        _layer = layer;
    }
    
    public bool IsDone()
    {
        AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(_layer);
        
        bool result = animatorStateInfo.fullPathHash == _hash && animatorStateInfo.normalizedTime >= 1 && 
                      _animator.IsInTransition(_layer) == false;
        
        return result;
    }
}
