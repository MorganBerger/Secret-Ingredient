using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomTrigger : MonoBehaviour
{
    private BoxCollider2D roomBoundary;
    private static CinemachineConfiner2D confiner;
    private bool isInitialized = false;
    private float lastChangeTime = 0f;
    private float changeCooldown = 0.3f;
    private float initialDamping = 3f;

    void Start()
    {
        if (confiner == null)
        {
            confiner = FindFirstObjectByType<CinemachineCamera>()
                .GetComponent<CinemachineConfiner2D>();
            confiner.Damping = initialDamping;
        }

        roomBoundary = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Ensure the confiner is set at the first frame
        if (isInitialized) return;
        CapsuleCollider2D playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
        if (!roomBoundary.IsTouching(playerCol)) return;
        confiner.BoundingShape2D = roomBoundary;
        isInitialized = true;
    }

    /// <summary>
    /// Called when the player enters the room's trigger area
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastChangeTime < changeCooldown) return;
            confiner.BoundingShape2D = roomBoundary;
            lastChangeTime = Time.time;
        }
    }
}