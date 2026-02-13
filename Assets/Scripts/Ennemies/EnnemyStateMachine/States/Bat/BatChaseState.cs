using UnityEngine;

public class BatChaseState: BatState
{
    private bool hasDetectedPlayer = false;

    public BatChaseState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
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

        if (bat.targetPlayer == null)
        {
            stateMachine.ChangeState(bat.idleState);
            return;
        }

        float distToPlayer = Vector2.Distance(bat.transform.position, bat.targetPlayer.transform.position);
        if (distToPlayer <= bat.attackRange)
        {
            stateMachine.ChangeState(bat.attackState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState) return;

        float dirToPlayer = bat.targetPlayer.transform.position.x - bat.transform.position.x;
        if ((dirToPlayer > 0 && bat.transform.localScale.x < 0) || (dirToPlayer < 0 && bat.transform.localScale.x > 0))
        {
            bat.Flip();
        }

        Collider2D playerCollider = bat.DetectPlayer();
        bat.targetPlayer = playerCollider != null ? playerCollider.gameObject : null;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        float moveDir = bat.transform.localScale.x;
        bat.rb.linearVelocity = new Vector2(moveDir * bat.chaseSpeed, bat.rb.linearVelocity.y);
    }
}