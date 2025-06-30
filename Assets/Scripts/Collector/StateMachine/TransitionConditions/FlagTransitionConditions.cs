using UnityEngine;

public class FlagTransitionConditions : ITransitionCondition
{
    private bool _flag;

    public bool IsDone()
    {
        Debug.Log("_flag " + _flag);
        if (_flag)
        {
            _flag = false;
            return true;
        }
        
        return _flag;
    }

    public void SetTrueFlag()
    {
        Debug.Log("set _flag " );
        _flag = true;
    }
}
