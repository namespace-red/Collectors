public class MoverToPointState : IState
{
    private readonly MoverToPoint _moverToPoint;

    public MoverToPointState(MoverToPoint moverToPoint)
    {
        _moverToPoint = moverToPoint;
    }
    
    public void Enter()
    {
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
