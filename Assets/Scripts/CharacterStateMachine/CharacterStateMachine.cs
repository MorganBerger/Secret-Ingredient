using UnityEngine;

public class CharacterStateMachine
{
    public CharacterState _CurrentState;

    public void InitializeStateMachine(CharacterState initialState){
        _CurrentState = initialState;
        _CurrentState.Enter();
    }

    public void ChangeState(CharacterState newState)
    {
        if (_CurrentState == newState) return;

        Debug.Log("Changing state from " + _CurrentState.GetType().Name + " to " + newState.GetType().Name);
        _CurrentState.Exit();
        _CurrentState = newState;
        _CurrentState.Enter();
    }
}