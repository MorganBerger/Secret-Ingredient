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

        if (character.attackAction.WasPressedThisFrame() && GetType() != typeof(WallSlideState))
        {
            stateMachine.ChangeState(character.groundAttackState);
            return;
        }

        var type = GetType();
        if (character.jumpBufferCounter > 0f && type != typeof(WallSlideState))
        {
            character.ConsumeJumpBuffer();
            stateMachine.ChangeState(character.jumpState);
            return;
        }

        if (!character.isParrying && character.parryAction.WasPressedThisFrame() && character.parryBufferCounter <= 0f)
        {
            stateMachine.ChangeState(character.parryState);
            return;
        }

        float moveInput = character.moveAction.ReadValue<Vector2>().x;

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

        if (character.drinkAction.WasPressedThisFrame())
        {
            stateMachine.ChangeState(character.drinkState);
        }
    }
}