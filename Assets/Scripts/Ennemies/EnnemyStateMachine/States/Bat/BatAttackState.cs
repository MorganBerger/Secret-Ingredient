using UnityEngine;

public class BatAttackState : BatState
{
    private bool attacked = false;
    private Vector2 targetPosition;

    public BatAttackState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        bat.rb.linearVelocity = Vector2.zero;
        targetPosition = (bat.targetPlayer.transform.position - bat.transform.position).normalized;
        FlipToPlayer();
    }

    private void FlipToPlayer()
    {
        if (bat.targetPlayer == null) return;

        Vector2 dirToPlayer = bat.targetPlayer.transform.position - bat.transform.position;
        if ((dirToPlayer.x > 0 && bat.transform.localScale.x < 0) || (dirToPlayer.x < 0 && bat.transform.localScale.x > 0))
        {
            bat.Flip();
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        stateMachine.ChangeState(bat.idleState);
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        if (isExitingState) return;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!attacked || isExitingState) return;
    }

    public override void AnimationEvent()
    {
        base.AnimationEvent();
        attacked = true;
        bat.rb.AddForce(targetPosition * bat.attackForce, ForceMode2D.Impulse);
    }
}