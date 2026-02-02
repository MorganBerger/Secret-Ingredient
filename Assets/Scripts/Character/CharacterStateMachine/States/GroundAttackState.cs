using UnityEngine;

public class GroundAttackState : CharacterState
{
    public GroundAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.rb.linearVelocity = new Vector2(0f, 0f);
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (isAnimationFinished) 
        {
            stateMachine.ChangeState(character.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}