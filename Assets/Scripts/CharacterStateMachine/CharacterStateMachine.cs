

public class CharacterStateMachine
{
    public CharacterState _CurrentState;

    public void InitializeStateMachine(CharacterState initialState){
        _CurrentState = initialState;
        _CurrentState.Enter();
    }

    public void ChangeState(CharacterState newState)
    {
        _CurrentState.Exit();
        _CurrentState = newState;
        _CurrentState.Enter();
    }
}