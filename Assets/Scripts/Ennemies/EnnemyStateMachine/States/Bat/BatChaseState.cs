using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BatChaseState: BatState
{
    private bool hasDetectedPlayer = false;
    private Vector2 dirToPlayer;
    private Collider2D[] rejectedColliders;

    public BatChaseState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hasDetectedPlayer = true;
        rejectedColliders = bat.GetComponentsInChildren<Collider2D>();
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(bat.transform.position, (bat.targetPlayer.transform.position - bat.transform.position).normalized, bat.attackRange);
            if (hits.Any(hit => hit.collider.gameObject.layer != LayerMask.NameToLayer("Player") && !rejectedColliders.Contains(hit.collider))) return;

            stateMachine.ChangeState(bat.attackState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState) return;

        dirToPlayer = bat.targetPlayer.transform.position - bat.transform.position;
        if ((dirToPlayer.x > 0 && bat.transform.localScale.x < 0) || (dirToPlayer.x < 0 && bat.transform.localScale.x > 0))
        {
            bat.Flip();
        }

        Collider2D playerCollider = bat.DetectPlayer();
        bat.targetPlayer = playerCollider != null ? playerCollider.gameObject : null;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector2 normalizedDir = dirToPlayer.normalized;
        bat.rb.linearVelocity = new Vector2(normalizedDir.x * bat.chaseSpeed, normalizedDir.y * bat.chaseSpeed);
    }
}