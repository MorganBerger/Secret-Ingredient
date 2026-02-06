using UnityEngine;

public class WolfPatrolState : WolfState
{
    private float startPosX;

    public WolfPatrolState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName) { }

    public override void Enter()
    {
        base.Enter();
        startPosX = wolf.transform.position.x;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (wolf.DetectPlayer() != null)
        {
            stateMachine.ChangeState(wolf.chaseState);
            return;
        }

        float distanceMoved = Mathf.Abs(wolf.transform.position.x - startPosX);

        if (distanceMoved >= wolf.patrolDistance || wolf.IsTouchingWall() || wolf.IsLedgeAhead())
        {
            stateMachine.ChangeState(wolf.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        float moveDir = wolf.transform.localScale.x;
        wolf.rb.linearVelocity = new Vector2(moveDir * wolf.speed, wolf.rb.linearVelocity.y);
    }
}