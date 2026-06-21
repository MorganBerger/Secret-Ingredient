using UnityEngine;

public class TrapDetectionZone : MonoBehaviour 
{
    private Trap parentTrap;
    
    void Start()
    {
        parentTrap = GetComponentInParent<Trap>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentTrap.OnPlayerDetected();
        }
    }
}
