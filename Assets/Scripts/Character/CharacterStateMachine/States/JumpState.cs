using UnityEngine;

public class JumpState : AirState
{
    private float jumpForce = 4f;
    private float jumpCooldown = 0.1f;

    private bool canTransition;

    public JumpState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        canTransition = false;

        character.rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (character.rb.linearVelocity.y <= 0.5f && canTransition)
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
        }

        if (Input.GetKeyUp(KeyCode.Z) && character.rb.linearVelocity.y > 0f)
        {
            character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
        }
    }
}