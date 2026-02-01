using UnityEngine;

public class DashState: CharacterState
{
    private float dashSpeed = 3f;
    // private float dashDuration = 0.35f;

    public DashState(Character _character, string _animationName) 
        : base(_character, _animationName) { }


    public override void Enter()
    {
        base.Enter();

        character.rb.linearVelocity = new Vector2(0f, 0f);
        character.rb.AddForce(new Vector2(character.transform.localScale.x * dashSpeed, 0f), ForceMode2D.Impulse);

        character.canDash = false;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        var touchingGround = character.IsTouchingGround();
        var touchingWall = character.IsTouchingWall();

        if (touchingWall && !touchingGround)
        {
            stateMachine.ChangeState(character.wallSlideState);
            return;
        }

        if (touchingGround && touchingWall)
        {
            stateMachine.ChangeState(character.idleState);
            return;
        }

        // if (Time.time >= startTime + dashDuration)
        if (isAnimationFinished)
        {
            if (!touchingGround) {
                stateMachine.ChangeState(character.fallState);
            } else {
                stateMachine.ChangeState(character.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isExitingState) return;

        // Neutralize any vertical velocity during dash 
        character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
    }
}