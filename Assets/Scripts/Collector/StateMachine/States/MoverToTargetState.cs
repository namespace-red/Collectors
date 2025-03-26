using System;
using UnityEngine;

public class MoverToTargetState : IState
{
    private readonly MoverToTarget _moverToTarget;

    private Transform _target;
    private Vector3 _offset;

    public event Action Finished;

    public MoverToTargetState(MoverToTarget moverToTarget)
    {
        _moverToTarget = moverToTarget ? moverToTarget : throw new NullReferenceException(nameof(moverToTarget));
    }

    public MoverToTargetState(MoverToTarget moverToTarget, IPosition position)
    {
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
