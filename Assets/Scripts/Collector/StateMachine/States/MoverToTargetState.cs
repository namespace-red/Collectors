using System;
using UnityEngine;

public class MoverToTargetState : IState
{
    private readonly CollectorAnimations _animations;
    private readonly MoverToTarget _moverToTarget;

    private Transform _target;
    private Vector3 _offset;

    public event Action Finished;

    public MoverToTargetState(CollectorAnimations animations, MoverToTarget moverToTarget)
    {
        _animations = animations ? animations : throw new NullReferenceException(nameof(animations));
        _moverToTarget = moverToTarget ? moverToTarget : throw new NullReferenceException(nameof(moverToTarget));
    }

    public MoverToTargetState(CollectorAnimations animations, MoverToTarget moverToTarget, IPosition position)
    {
        _animations = animations ? animations : throw new NullReferenceException(nameof(animations));
        _moverToTarget = moverToTarget ? moverToTarget : throw new NullReferenceException(nameof(moverToTarget));
        Target = position.Transform;
        _offset = position.Offset;
    }
    
    public Transform Target
    {
        get => _target;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _target = value;
        }
    }

    public void Enter()
    {
        _animations.PlayRun();
        
        _moverToTarget.Target = Target;
        _moverToTarget.Offset = _offset;
        _moverToTarget.enabled = true;
    }

    public void Exit()
    {
        _moverToTarget.enabled = false;
        Finished?.Invoke();
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
