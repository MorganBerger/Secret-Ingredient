public class CharacterState: State
{
    protected Character character;

    public CharacterState(Character _character, string _animationName)
        : base(_character.stateMachine, _character.anim, _animationName)
    {
        character = _character;
    }

    public override void Enter()
    {
        base.Enter();
        character.currentState = GetType().Name;
    }
}