using System;

public class MoverToPointState : IState
{
    private readonly MoverToPoint _moverToPoint;
    private readonly IPosition _position;

    public MoverToPointState(MoverToPoint moverToPoint, IPosition position)
    {
        _moverToPoint = moverToPoint ? moverToPoint : throw new NullReferenceException(nameof(moverToPoint));
        _position = position ?? throw new NullReferenceException(nameof(position));
    }
    
    public void Enter()
    {
        _moverToPoint.TargetPoint = _position.Get();
        _moverToPoint.enabled = true;
    }

    public void Exit()
    {
        _moverToPoint.enabled = false;
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
