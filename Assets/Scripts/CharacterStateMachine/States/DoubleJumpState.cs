using UnityEngine;

public class DoubleJumpState : JumpState
{
    public DoubleJumpState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}