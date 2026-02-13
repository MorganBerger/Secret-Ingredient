using UnityEngine;

public class BatFallState : BatState
{
    public BatFallState(Bat _bat, string _animationName): base(_bat, _animationName) { }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        bat.rb.linearVelocity = new Vector2(0, -bat.fallSpeed);

    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (bat.IsGrounded())
        {
            stateMachine.ChangeState(bat.deathState);
        }
    }
}