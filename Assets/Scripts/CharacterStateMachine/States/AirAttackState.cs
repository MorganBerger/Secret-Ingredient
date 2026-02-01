using UnityEngine;

public class AirAttackState : CharacterState
{
    public AirAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
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
}