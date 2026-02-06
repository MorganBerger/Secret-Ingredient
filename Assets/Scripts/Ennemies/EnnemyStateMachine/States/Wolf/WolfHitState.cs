using UnityEngine;

public class WolfHitState: WolfState
{
    public WolfHitState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        
        stateMachine.ChangeState(wolf.idleState);
    }
}