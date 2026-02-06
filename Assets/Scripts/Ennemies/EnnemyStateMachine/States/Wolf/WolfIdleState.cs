using UnityEngine;

public class WolfIdleState : WolfState
{
    public WolfIdleState(Wolf _wolf, string _animationName)
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

        // if (Time.time >= startTime + wolf.patrolPauseTime)
        // {
        //     stateMachine.ChangeState(wolf.runState);
        // }
    }
}