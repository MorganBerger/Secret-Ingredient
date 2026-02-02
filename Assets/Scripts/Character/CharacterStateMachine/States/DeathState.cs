using UnityEngine;

public class DeathState : CharacterState
{
    public DeathState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.rb.linearVelocity = Vector2.zero;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.rb.linearVelocity = Vector2.zero;
    }
}