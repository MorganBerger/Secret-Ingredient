using UnityEngine;

public class GroundedState : MovementState
{
    public GroundedState(Character _character, string _animationName) 
        : base(_character, _animationName) { }

    public override void Enter()
    {
        base.Enter();
        character.canDoubleJump = true;
        character.canDash = true;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (Input.GetKeyDown(KeyCode.T) && GetType() != typeof(WallSlideState))
        {
            stateMachine.ChangeState(character.groundAttackState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(character.jumpState);
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Approximately(moveInput, 0f) && Mathf.Approximately(character.rb.linearVelocity.y, 0f))
        {
            stateMachine.ChangeState(character.idleState);
            return;
        }

        if (character.rb.linearVelocity.y < -0.1f && !character.IsTouchingGround() && !character.IsTouchingWall())
        {
            stateMachine.ChangeState(character.peakState);
            return;
        }
    }
}