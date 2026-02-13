using UnityEngine;

public class AirAttackState : AirState //CharacterState
{
    public AirAttackState(Character _character, string _animationName)
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
}