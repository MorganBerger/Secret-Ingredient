using UnityEngine;

public class StateMachine
{
    public State _CurrentState;

    public virtual void InitializeStateMachine(State initialState){
        _CurrentState = initialState;
        _CurrentState.Enter();
    }

    public virtual void ChangeState(State newState)
    {
        if (_CurrentState == newState) return;

        // Debug.Log("Changing state from " + _CurrentState.GetType().Name + " to " + newState.GetType().Name);
        _CurrentState.Exit();
        _CurrentState = newState;
        _CurrentState.Enter();
    }
}