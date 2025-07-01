public class FlagTransitionConditions : ITransitionCondition
{
    private bool _flag;

    public bool IsDone()
    {
        if (_flag)
        {
            _flag = false;
            return true;
        }
        
        return _flag;
    }

    public void SetTrueFlag()
    {
        _flag = true;
    }
}
