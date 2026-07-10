using UnityEngine;

public class AirAttackState : AirState
{
    public AirAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.PlayAttackSound();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(character.fallState);
    }
}