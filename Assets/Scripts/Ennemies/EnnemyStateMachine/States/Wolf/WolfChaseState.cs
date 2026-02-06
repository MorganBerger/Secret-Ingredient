using UnityEngine;

public class WolfChaseState: WolfState
{
    public WolfChaseState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // Collider2D playerCollider = wolf.DetectPlayer();

        // if (playerCollider == null)
        // {
        //     stateMachine.ChangeState(wolf.idleState);
        //     return;
        // }

        // float distToPlayer = Vector2.Distance(wolf.transform.position, playerCollider.transform.position);
        // if (distToPlayer <= wolf.attackRange)
        // {
        //     stateMachine.ChangeState(wolf.attackState);
        //     return;
        // }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState) return;

        Collider2D playerCollider = wolf.DetectPlayer();

        if (playerCollider == null)
        {
            stateMachine.ChangeState(wolf.idleState);
            return;
        }

        float distToPlayer = Vector2.Distance(wolf.transform.position, playerCollider.transform.position);
        if (distToPlayer <= wolf.attackRange)
        {
            stateMachine.ChangeState(wolf.attackState);
            return;
        }

        float dirToPlayer = playerCollider.transform.position.x - wolf.transform.position.x;
        if ((dirToPlayer > 0 && wolf.transform.localScale.x < 0) || (dirToPlayer < 0 && wolf.transform.localScale.x > 0))
        {
            wolf.Flip();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Chase at high speed, but stop at ledges
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