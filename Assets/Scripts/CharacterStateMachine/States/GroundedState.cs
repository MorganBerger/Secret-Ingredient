using UnityEngine;

public class GroundedState : MovementState
{
    public GroundedState(Character _character, string _animationName) 
        : base(_character, _animationName) { }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stateMachine.ChangeState(character.jumpState);
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Approximately(moveInput, 0f) && Mathf.Approximately(character.rb.linearVelocity.y, 0f))
        {
            stateMachine.ChangeState(character.idleState);
            return;
        }

        if (character.rb.linearVelocity.y < -0.1f)
        {
            stateMachine.ChangeState(character.fallState);
            return;
        }
    }
}