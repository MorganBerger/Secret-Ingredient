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

        if (isExitingState) return;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        stateMachine.ChangeState(character.idleState);
    }
}