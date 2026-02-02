using UnityEngine;

public class HurtState : CharacterState
{
    public HurtState(Character _character, string _animationName)
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
            var isTouchingGround = character.IsTouchingGround();            

            if (isTouchingGround)
            {
                stateMachine.ChangeState(character.idleState);
            }
            else
            {
                stateMachine.ChangeState(character.peakState);
            }
        }
    }
}