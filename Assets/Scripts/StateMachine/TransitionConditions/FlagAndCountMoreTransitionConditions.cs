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
        bool isFlagDone = _flagTc.IsDone();
        bool isCountMore = _countMoreTc.IsDone();
        
        if (isFlagDone && isCountMore == false)
            _flagTc.SetTrueFlag();
        
        return isFlagDone && isCountMore;
    }

    public void SetTrueFlag()
        => _flagTc.SetTrueFlag();
}
