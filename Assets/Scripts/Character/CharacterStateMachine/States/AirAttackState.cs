using UnityEngine;

public class AirAttackState : AirState //CharacterState
{
    public AirAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        stateMachine.ChangeState(character.fallState);
    }

    // public override void PhysicsUpdate()
    // {
    //     base.PhysicsUpdate();
    //     if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space)) && character.rb.linearVelocity.y > 0f)
    //     {
    //         character.rb.linearVelocity = new Vector2(character.rb.linearVelocity.x, 0f);
    //     }
    // }
}