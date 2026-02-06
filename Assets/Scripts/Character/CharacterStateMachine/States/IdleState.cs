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

        if (character.isDead())
        {
            stateMachine.ChangeState(character.deathState);
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!Mathf.Approximately(moveInput, 0f))
        {
            stateMachine.ChangeState(character.runState);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            stateMachine.ChangeState(character.drinkState);
        }
    }
}