using System.Collections;

public class FlagAndCountMoreTransitionConditions : ITransitionCondition
{
    private FlagTransitionConditions _flagTc;
    private CountMoreTransitionConditions _countMoreTc;

    public FlagAndCountMoreTransitionConditions(ICollection collection, int count)
    {
        _flagTc = new FlagTransitionConditions();
        _countMoreTc = new CountMoreTransitionConditions(collection, count);
    }
    
    public bool IsDone()
    {
        return _flagTc.IsDone() && _countMoreTc.IsDone();
    }

    public void SetTrueFlag()
        => _flagTc.SetTrueFlag();
}
