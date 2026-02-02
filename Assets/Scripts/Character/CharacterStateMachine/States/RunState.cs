using UnityEngine;

public class RunState: GroundedState
{
    private float moveSpeed = 2f;

    public RunState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isExitingState) return;
    }
}