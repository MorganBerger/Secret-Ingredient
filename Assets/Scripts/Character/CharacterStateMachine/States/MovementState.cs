using SomeExtensions;
using UnityEngine;

public class MovementState : CharacterState
{
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

        if (character.dashAction.WasPressedThisFrame() && character.canDash && !character.IsDead())
        {
            stateMachine.ChangeState(character.dashState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
                
        float horizontalInput = character.moveAction.ReadValue<Vector2>().x;

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

        if (character.IsDead()) return;
        
        float horizontalInput = character.moveAction.ReadValue<Vector2>().x;

        var velocity = horizontalInput.Raw() * character.speed;

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