using UnityEngine;

public class FallState : MovementState
{
    public FallState(Character _character, string _animationName) : base(_character, _animationName) { }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (character.IsTouchingGround())
        {
            stateMachine.ChangeState(character.idleState);
        }
    }
}