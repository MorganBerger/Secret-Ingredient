using UnityEngine;

public class CharacterState
{
    protected Character character;
    protected CharacterStateMachine stateMachine;
    protected Animator animator;
    protected string animationName;

    protected bool isExitingState;
    protected bool isAnimationFinished;
    protected float startTime;
    
    public CharacterState(Character _character, CharacterStateMachine _stateMachine, Animator _animator, string _animationName)
    {
        character = _character;
        stateMachine = _stateMachine;
        animator = _animator;
        animationName = _animationName;
    }

    public virtual void Enter()
    {
        isAnimationFinished = false;
        isExitingState = false;
        startTime = Time.time;
        animator.SetBool(animationName, true);
    }
    public virtual void Exit()
    {
        isExitingState = true;
        if (!isAnimationFinished) isAnimationFinished = true;
        animator.SetBool(animationName, true);   
    }
    public virtual void LogicUpdate()
    {
        TransitionChecks();
    }
    public virtual void PhysicsUpdate()
    {
        
    }
    public virtual void TransitionChecks()
    {
        
    }
    public virtual void AnimationTrigger()
    {
        isAnimationFinished = true;
    }
}