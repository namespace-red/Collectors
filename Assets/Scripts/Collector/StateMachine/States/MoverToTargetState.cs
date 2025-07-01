using System;

public class MoverToTargetState : IState
{
    private readonly CollectorAnimations _animations;
    private readonly MoverToTarget _moverToTarget;
    private  IPosition _target;

    public MoverToTargetState(CollectorAnimations animations, MoverToTarget moverToTarget, IPosition target)
    {
        _animations = animations ? animations : throw new ArgumentNullException(nameof(animations));
        _moverToTarget = moverToTarget ? moverToTarget : throw new ArgumentNullException(nameof(moverToTarget));
        _target = target ?? throw new ArgumentNullException(nameof(target));
    }
   
    public void Enter()
    {
        _animations.PlayRun();
        
        _moverToTarget.Target = _target.Transform;
        _moverToTarget.Offset = _target.Offset;
        _moverToTarget.enabled = true;
    }

    public void Exit()
    {
        _moverToTarget.enabled = false;
    }
    
    public void FixedUpdate()
    {
    }

    public void Update()
    {
    }
}
