using UnityEngine;

public class MovementState : CharacterState
{
    private float moveSpeed = 2f;

    protected bool canGoRight;
    protected bool canGoLeft;

    public MovementState(Character _character, string _animationName) 
        : base(_character, _animationName) { }

    public override void Enter()
    {
        base.Enter();

        canGoRight = true;
        canGoLeft = true;
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(character.dashState);
            return;
        }

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
        {
            character.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            character.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        var velocity = horizontalInput * moveSpeed;

        if (!canGoRight && velocity > 0)
        {
            velocity = character.rb.linearVelocity.x;
        }
        
        if (!canGoLeft && velocity < 0)
        {
            velocity = character.rb.linearVelocity.x;
        }

        character.rb.linearVelocity = new Vector2(velocity, character.rb.linearVelocity.y);
    }
}