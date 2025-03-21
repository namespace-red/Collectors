public class FlagTransitionConditions : ITransitionCondition
{
    public bool Flag;
    
    public bool IsDone()
    {
        if (Flag)
        {
            Flag = false;
            return true;
        }
        
        return Flag;
    }
}
