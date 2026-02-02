using UnityEngine;

public class WolfIdleState : WolfState
{
    public WolfIdleState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;
    }
}