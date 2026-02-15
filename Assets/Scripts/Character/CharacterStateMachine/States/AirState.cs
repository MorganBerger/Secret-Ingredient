using UnityEngine;

public class AirState : MovementState
{
    private bool canCheckForGround;

    public AirState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCheckForGround = false;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        if (isExitingState) return;

        var touchingGround = character.IsTouchingGround();
        var touchingWall = character.IsTouchingWall();

        if (touchingGround && canCheckForGround)
        {
            stateMachine.ChangeState(character.idleState);
        }

        if (character.isDead()) return;

        if (Input.GetKeyDown(KeyCode.T) && !touchingGround)
        {
            stateMachine.ChangeState(character.airAttackState);
            return;
        }

        var direction = character.transform.localScale.x;
        if (touchingWall && !touchingGround && Input.GetAxisRaw("Horizontal") == direction)
        {
            stateMachine.ChangeState(character.wallSlideState);
        }

        var type = GetType();
        if (character.canDoubleJump && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space)) && type != typeof(WallSlideState))
        {
            stateMachine.ChangeState(character.doubleJumpState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        var type = GetType();
        if (type == typeof(PeakState) || type == typeof(FallState) || type == typeof(WallSlideState))
        {
            canCheckForGround = true;
        }

        if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space)) && character.rb.linearVelocity.y > 0f)
        {
            character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
        }
    }
}