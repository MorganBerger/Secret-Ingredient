using UnityEngine;

public class WolfAttackState : WolfState
{
    public WolfAttackState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        wolf.rb.linearVelocity = Vector2.zero;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(wolf.idleState);
        }
    }
}