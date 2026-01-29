using UnityEngine;

public class MovementState : CharacterState
{
    private float moveSpeed = 2f;

    public MovementState(Character _character, string _animationName) 
        : base(_character, _animationName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
            character.transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            character.transform.localScale = new Vector3(-1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        character.rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, character.rb.linearVelocity.y);
    }
}