using System;

public class MoverToTargetState : IState
{
    private readonly MoverToTarget _moverToTarget;

    public MoverToTargetState(MoverToTarget moverToTarget)
    {
        _moverToTarget = moverToTarget ? moverToTarget : throw new NullReferenceException(nameof(moverToTarget));
    }

    public void Enter()
    {
        _moverToTarget.enabled = true;
    }

    public void Exit()
    {
        _moverToTarget.enabled = false;
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
