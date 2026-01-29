using UnityEngine;

public class JumpState : MovementState
{
    private float jumpForce = 200f;

    private float jumpCooldown = 0.1f; // 0.1 seconds to allow velocity to build
    private bool canTransition;

    public JumpState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    // public override void Enter()
    // {
    //     base.Enter();
    //     character.rb.AddForce(new Vector2(0f, jumpForce));
    // }

    public override void Enter()
    {
        base.Enter();
        
        canTransition = false;
        
        // Reset vertical velocity for a consistent jump
        // character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
        
        // Use Impulse for immediate velocity change
        character.rb.AddForce(new Vector2(0f, jumpForce));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Start allowing transition checks after a tiny delay
        if (!canTransition && Time.time >= startTime + jumpCooldown)
        {
            canTransition = true;
        }

        if (Input.GetKeyUp(KeyCode.Z) && character.rb.linearVelocity.y > 0f)
        {
            Debug.Log("Jump Key Released Early");
            character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (character.rb.linearVelocity.y <= 0.5f && canTransition)
        {
            stateMachine.ChangeState(character.peakState);
        }
    }
}