using UnityEngine;

public class PeakState : AirState
{
    public PeakState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(character.fallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (character.health <= 0)
        {
            character.rb.linearVelocity = new Vector2(0, character.rb.linearVelocity.y);
        }
    }
}