using UnityEngine;

public class WallJumpState : AirState
{
    private float wallJumpForceX = 1.5f;
    private float wallJumpForceY = 3.5f;

    private float jumpCooldown = .25f;
    private bool canTransition;

    private float jumpDirection;

    public WallJumpState(Character _character, string _animationName) 
        : base(_character, _animationName) { }

    public override void Enter()
    {
        base.Enter();

        jumpDirection = -character.transform.localScale.x;

        canTransition = false;

        if (jumpDirection > 0)
        {
            canGoLeft = false;
        }
        else
        {
            canGoRight = false;
        }

        character.rb.linearVelocity = Vector2.zero;
        character.rb.AddForce(new Vector2(jumpDirection * wallJumpForceX, wallJumpForceY), ForceMode2D.Impulse);
    }

    public override void TransitionChecks()
    {
        if (isExitingState) return;

        if (!canTransition) return;
        
        base.TransitionChecks();

        if (character.rb.linearVelocity.y <= 0.5f)
        {
            stateMachine.ChangeState(character.peakState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!canTransition && Time.time >= startTime + jumpCooldown)
        {
            canTransition = true;
            canGoLeft = true;
            canGoRight = true;
        }

        if (!canTransition)
        {
            character.transform.localScale = new Vector3(jumpDirection, 1, 1);
        }
    }

    public override void Exit()
    {
        base.Exit();
        canGoLeft = true;
        canGoRight = true;
    }
}