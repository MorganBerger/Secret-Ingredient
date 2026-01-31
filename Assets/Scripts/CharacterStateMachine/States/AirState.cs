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
        if (isExitingState) return;

        base.TransitionChecks();

        var touchingGround = character.IsTouchingGround();
        var touchingWall = character.IsTouchingWall();

        if (touchingGround && canCheckForGround)
        {
            stateMachine.ChangeState(character.idleState);
        }

        var direction = character.transform.localScale.x;
        if (touchingWall && !touchingGround && Input.GetAxisRaw("Horizontal") == direction)
        {
            stateMachine.ChangeState(character.wallSlideState);
        }

        var type = GetType();
        if (character.canDoubleJump && Input.GetKeyDown(KeyCode.Z) && type != typeof(WallSlideState))
        {
            stateMachine.ChangeState(character.doubleJumpState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        var type = GetType();
        if (type == typeof(FallState) || type == typeof(WallSlideState))
        {
            canCheckForGround = true;
        }
    }
}