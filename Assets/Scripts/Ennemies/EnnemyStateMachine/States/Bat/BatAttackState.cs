using UnityEngine;

public class BatAttackState : BatState
{
    public BatAttackState(Bat _bat, string _animationName)
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

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(bat.idleState);
        }
    }
}