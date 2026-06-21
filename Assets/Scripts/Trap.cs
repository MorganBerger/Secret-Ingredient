using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Trap : MonoBehaviour
{
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected bool applyRespawn = false;
    [SerializeField] protected float cooldownTime = .5f;
    [SerializeField] protected bool auto = false;
    protected Animator animator;
    protected bool isOnCooldown = false;
    protected bool isTriggered = false;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (auto && !isTriggered)
        {
            Debug.Log("Auto-triggering trap");
            TriggerTrap();
        }
    }

    public virtual void OnPlayerDetected()
    {
        TriggerTrap();
    }

    public virtual void OnPlayerTouchTrap(Collider2D col)
    {
        if (col.TryGetComponent<Character>(out var player))
        {
            player.TakeDamage(damage);
            if (applyRespawn)
            {
                GameManager.Instance.Respawn();
            }
        }
    }

    protected virtual void TriggerTrap()
    {
        if (isTriggered) return;
        isTriggered = true;
        animator.SetBool("isTriggered", true);
    }

    protected virtual void OnAnimationEnd()
    {
        Invoke(nameof(ResetTrap), cooldownTime);
    }

    protected virtual void ResetTrap()
    {
        Debug.Log("Resetting trap");
        animator.SetBool("isTriggered", false);
        isTriggered = false;
    }
}