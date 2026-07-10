using System;
using UnityEngine;
using UnityEngine.InputSystem;


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

        if (character.IsDead())
        {
            stateMachine.ChangeState(character.deathState);
            return;
        }

        float moveInput = character.moveAction.ReadValue<Vector2>().x;

        if (!Mathf.Approximately(moveInput, 0f))
        {
            stateMachine.ChangeState(character.runState);
        }
    }
}