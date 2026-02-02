using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float speed = 2f;

    public StateMachine stateMachine { get; protected set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        stateMachine = new StateMachine();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Attack(Character character)
    {
        // character.TakeDamage(damage);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Destroy(gameObject);
    }

    void AnimationFinished()
    {
        stateMachine._CurrentState.AnimationTrigger();
    }
}
