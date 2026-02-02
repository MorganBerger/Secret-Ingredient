public class CharacterStateMachine: StateMachine
{
    public override void ChangeState(State newState)
    {
        if (!CanChangeState(newState.GetType())) return;

        base.ChangeState(newState);
    }
    
    public bool CanChangeState(System.Type type)
    {
        switch (type)
        {
            case var t when t == typeof(DashState):
                if (!CharacterSkills.canDash) return false;
                break;
            case var t when t == typeof(DoubleJumpState):
                if (!CharacterSkills.canDoubleJump) return false;
                break;
            case var t when t == typeof(WallSlideState):
                if (!CharacterSkills.canWallClimb) return false;
                break;
        }
        return true;
    }
}