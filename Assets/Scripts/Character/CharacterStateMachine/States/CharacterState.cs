public class CharacterState: State
{
    protected Character character;

    public CharacterState(Character _character, string _animationName)
    {
        character = _character;
        stateMachine = _character.stateMachine;
        animator = _character.anim;
        animationName = _animationName;
    }
}