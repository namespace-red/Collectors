using UnityEngine;

public class AnimatedTransitionConditions : ITransitionCondition
{
    private Animation _animation;
    
    public AnimatedTransitionConditions(Animation animation)
    {
        _animation = animation;
    }
    
    public bool IsDone()
    {
        return _animation.isPlaying == false;
    }
}
