public class FlagTransitionConditions : ITransitionCondition
{
    public bool Flag;
    
    public bool IsDone()
    {
        return Flag;
    }
}
