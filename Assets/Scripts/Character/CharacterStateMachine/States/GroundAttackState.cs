using UnityEngine;

public class GroundAttackState : MovementState//CharacterState
{
    public GroundAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(character.idleState);
            return;
        }
    }
}