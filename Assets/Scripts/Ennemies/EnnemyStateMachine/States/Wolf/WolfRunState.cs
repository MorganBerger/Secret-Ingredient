using UnityEngine;

public class WolfRunState: WolfState
{
    private float startPosX;
    private bool isChasing;

    public WolfRunState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        startPosX = wolf.transform.position.x;
        isChasing = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) return;

        Collider2D player = wolf.DetectPlayer();

        if (player != null)
        {
            // AGGRO MODE
            isChasing = true;
            float distanceToPlayer = Vector2.Distance(wolf.transform.position, player.transform.position);

            if (distanceToPlayer <= wolf.attackRange)
            {
                stateMachine.ChangeState(wolf.attackState);
                return;
            }
        }
        else
        {
            // PATROL MODE
            isChasing = false;
            float distanceCovered = Mathf.Abs(wolf.transform.position.x - startPosX);

            if (distanceCovered >= wolf.patrolDistance || wolf.IsTouchingWall() || wolf.IsLedgeAhead())
            {
                wolf.Flip();
                stateMachine.ChangeState(wolf.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float moveDir = wolf.transform.localScale.x;
        float speed = isChasing ? wolf.chaseSpeed : wolf.speed;

        if (isChasing && wolf.IsLedgeAhead())
        {
            Debug.Log("Ledge ahead while chasing, stopping movement.");
            wolf.rb.linearVelocity = new Vector2(0, wolf.rb.linearVelocity.y);
            isChasing = false;

            stateMachine.ChangeState(wolf.idleState);
        }
        else
        {
            wolf.rb.linearVelocity = new Vector2(moveDir * speed, wolf.rb.linearVelocity.y);
        }
    }
}