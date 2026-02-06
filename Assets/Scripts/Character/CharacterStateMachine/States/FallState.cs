using UnityEngine;

public class FallState : AirState
{
    public FallState(Character _character, string _animationName) : base(_character, _animationName) { }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (character.health <= 0)
        {
            character.rb.linearVelocity = new Vector2(0, character.rb.linearVelocity.y);
        }
    }
}