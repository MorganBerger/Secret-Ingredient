using UnityEngine;

public class DrinkState : CharacterState
{
    float drinkDuration = 1.1f;

    public DrinkState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(character.idleState);
        }
    }
}