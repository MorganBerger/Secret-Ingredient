using UnityEngine;

public class DashState: CharacterState
{
    private float dashSpeed = 5f;
    private float dashDuration = 0.35f;

    public DashState(Character _character, string _animationName) 
        : base(_character, _animationName) { }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + dashDuration)
        {
            stateMachine.ChangeState(character.idleState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float direction = character.transform.localScale.x;
        character.rb.linearVelocity = new Vector2(direction * dashSpeed, character.rb.linearVelocity.y);
    }
}