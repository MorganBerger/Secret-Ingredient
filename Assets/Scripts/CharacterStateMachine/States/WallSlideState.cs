using UnityEngine;

public class WallSlideState : AirState
{
    private float slideSpeed = 0.5f;

    public WallSlideState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.canDoubleJump = true;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stateMachine.ChangeState(character.wallJumpState);
            return;
        }

        if (!character.IsTouchingWall() || Input.GetAxisRaw("Horizontal") == 0)
        {
            stateMachine.ChangeState(character.fallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float currentYVelocity = character.rb.linearVelocity.y;

        if (currentYVelocity < -slideSpeed)
        {
            character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, -slideSpeed);
        }
        else
        {
            character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, currentYVelocity);
        }
    }

}