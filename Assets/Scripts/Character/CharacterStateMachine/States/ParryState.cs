using UnityEngine;

public class ParryState: CharacterState
{
    public ParryState(Character _character, string _animationName) 
        : base(_character, _animationName) { }

    override public void Enter()
    {
        base.Enter();

        character.rb.linearVelocity = Vector2.zero;
        character.isParrying = true;
    }

    override public void Exit()
    {
        base.Exit();
        character.isParrying = false;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        stateMachine.ChangeState(character.idleState);
        character.ResetParryBuffer();
    }
}