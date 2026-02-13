using UnityEngine;

public class AirAttackState : AirState //CharacterState
{
    public AirAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        stateMachine.ChangeState(character.fallState);
    }
}