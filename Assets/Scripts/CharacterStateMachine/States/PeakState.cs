using UnityEngine;

public class PeakState : AirState
{
    private float minPeakDuration = 0.15f;

    public PeakState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (Time.time >= startTime + minPeakDuration)
        {
            stateMachine.ChangeState(character.fallState);
        }
    }
}