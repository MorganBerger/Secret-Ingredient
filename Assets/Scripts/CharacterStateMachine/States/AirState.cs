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

        if (character.IsTouchingGround() && canCheckForGround)
        {
            stateMachine.ChangeState(character.idleState);
        }

        if (character.IsTouchingWall() && Input.GetAxisRaw("Horizontal") != 0)
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