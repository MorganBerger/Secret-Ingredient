using UnityEngine;

public class WolfChaseState: WolfState
{
    private bool hasDetectedPlayer = false;

    public WolfChaseState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hasDetectedPlayer = true;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (wolf.targetPlayer == null)
        {
            stateMachine.ChangeState(wolf.idleState);
            return;
        }

        float distToPlayer = Vector2.Distance(wolf.transform.position, wolf.targetPlayer.transform.position);
        if (distToPlayer <= wolf.attackRange)
        {
            stateMachine.ChangeState(wolf.attackState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState) return;

        float dirToPlayer = wolf.targetPlayer.transform.position.x - wolf.transform.position.x;
        if ((dirToPlayer > 0 && wolf.transform.localScale.x < 0) || (dirToPlayer < 0 && wolf.transform.localScale.x > 0))
        {
            wolf.Flip();
        }

        var playerCollider = wolf.DetectPlayer();
        wolf.targetPlayer = playerCollider != null ? playerCollider.gameObject : null;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (wolf.IsLedgeAhead())
        {
            wolf.rb.linearVelocity = new Vector2(0, wolf.rb.linearVelocity.y);
        }
        else
        {
            float moveDir = wolf.transform.localScale.x;
            wolf.rb.linearVelocity = new Vector2(moveDir * wolf.chaseSpeed, wolf.rb.linearVelocity.y);
        }
    }
}