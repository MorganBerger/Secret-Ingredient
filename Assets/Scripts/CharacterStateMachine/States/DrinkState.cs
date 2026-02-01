using UnityEngine;

public class DrinkState : GroundedState
{
    public DrinkState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;
    }
}