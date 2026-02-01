using UnityEngine;

public struct CharacterSkills
{
    public static bool canDash = false;
    public static bool canDoubleJump = false;
    public static bool canWallClimb = false;
}

public class CharacterStateMachine
{
    public CharacterState _CurrentState;

    public void InitializeStateMachine(CharacterState initialState){
        _CurrentState = initialState;
        _CurrentState.Enter();
    }

    public void ChangeState(CharacterState newState)
    {
        if (_CurrentState == newState || !CanChangeState(newState.GetType())) return;

        Debug.Log("Changing state from " + _CurrentState.GetType().Name + " to " + newState.GetType().Name);
        _CurrentState.Exit();
        _CurrentState = newState;
        _CurrentState.Enter();
    }

    public bool CanChangeState(System.Type type)
    {
        switch (type)
        {
            case var t when t == typeof(DashState):
                if (!CharacterSkills.canDash) return false;
                break;
            case var t when t == typeof(DoubleJumpState):
                if (!CharacterSkills.canDoubleJump) return false ;
                break;
            case var t when t == typeof(WallSlideState):
                if (!CharacterSkills.canWallClimb) return false;
                break;
        }
        return true;
    }
}