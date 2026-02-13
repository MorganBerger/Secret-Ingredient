using UnityEngine;

public class DashState: CharacterState
{
    private float dashSpeed = 6f;

    private float dashDuration = 0.5f;

    public DashState(Character _character, string _animationName) 
        : base(_character, _animationName) { }


    public override void Enter()
    {
        base.Enter();

        character.rb.linearVelocity = new Vector2(0f, 0f);
        character.rb.AddForce(new Vector2(character.transform.localScale.x * dashSpeed, 0f), ForceMode2D.Impulse);

        character.canDash = false;

        character.rb.gravityScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        character.rb.gravityScale = 1f;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        
        if (isExitingState) return;

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

        var velocity = character.transform.localScale.x * character.speed;

        var timeElapsed = Time.time - startTime;
        var lerpedVelocity = Mathf.Lerp(character.rb.linearVelocity.x, velocity, timeElapsed / dashDuration);

        character.rb.linearVelocity = new Vector2(lerpedVelocity, 0f);
    }
}