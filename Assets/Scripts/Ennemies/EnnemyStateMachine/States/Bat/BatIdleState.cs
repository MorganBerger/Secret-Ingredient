using UnityEngine;

public class BatIdleState : BatState
{
    public BatIdleState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bat.rb.linearVelocity = Vector2.zero;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (bat.targetPlayer != null)
        {
            stateMachine.ChangeState(bat.chaseState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) return;

        Collider2D playerCollider = bat.DetectPlayer(ahead: true);
        bat.targetPlayer = playerCollider != null ? playerCollider.gameObject : null;
    }
}