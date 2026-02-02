using UnityEngine;

public class WolfAttackState : WolfState
{
    public WolfAttackState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        // stateMachine.ChangeState(wolf.idleState);
    }
}