using UnityEngine;

public class AirState : MovementState
{

    private float airCooldown = 0.1f;
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

        var touchingGround = character.IsTouchingGround();
        var touchingWall = character.IsTouchingWall();

        if (touchingGround && canCheckForGround)
        {
            stateMachine.ChangeState(character.idleState);
        }

        if (touchingWall && !touchingGround && Input.GetAxisRaw("Horizontal") != 0)
        {
            stateMachine.ChangeState(character.wallSlideState);
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