using UnityEngine;

public class SafePosition : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.lastSafePosition = transform.position;
        }
    }
};
