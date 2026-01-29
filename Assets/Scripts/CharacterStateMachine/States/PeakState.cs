using UnityEngine;

public class PeakState : MovementState
{
    private float minPeakDuration = 0.1f;

    public PeakState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Time.time >= startTime + minPeakDuration)
        {
            stateMachine.ChangeState(character.fallState);
        }
    }
}