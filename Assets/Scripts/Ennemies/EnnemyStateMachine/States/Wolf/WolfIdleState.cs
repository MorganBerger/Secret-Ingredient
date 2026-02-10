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

        if (wolf.targetPlayer != null)
        {
            stateMachine.ChangeState(wolf.chaseState);
            return;
        }

        if (Time.time > startTime + wolf.patrolPauseTime)
        {
            wolf.Flip();
            stateMachine.ChangeState(wolf.patrolState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) return;

        var playerCollider = wolf.DetectPlayer(ahead: true);
        wolf.targetPlayer = playerCollider != null ? playerCollider.gameObject : null;
    }
}