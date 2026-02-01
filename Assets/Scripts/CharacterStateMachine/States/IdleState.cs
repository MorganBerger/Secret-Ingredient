using System;
using UnityEngine;


public class IdleState : GroundedState
{
    public IdleState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return; 

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!Mathf.Approximately(moveInput, 0f))
        {
            stateMachine.ChangeState(character.runState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}