using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplate roomTemplate;
    private int rand;
    private bool spawned = false;
    private int maxRooms = 20;
    private static int currentRoomCount = 0;
    private List<Vector2> spawnedPositions = new List<Vector2>();

    private void Start()
    {
        roomTemplate = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (!spawned && currentRoomCount < maxRooms)
        {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, roomTemplate.bottomRooms.Length);
                Instantiate(roomTemplate.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            if (openingDirection == 2)
            {
                rand = Random.Range(0, roomTemplate.topRooms.Length);
                Instantiate(roomTemplate.topRooms[rand], transform.position, Quaternion.identity);
            }
            if (openingDirection == 3)
            {
                rand = Random.Range(0, roomTemplate.leftRooms.Length);
                Instantiate(roomTemplate.leftRooms[rand], transform.position, Quaternion.identity);
            }
            if (openingDirection == 4)
            {
                rand = Random.Range(0, roomTemplate.rightRooms.Length);
                Instantiate(roomTemplate.rightRooms[rand], transform.position, Quaternion.identity);
            }

            spawned = true;
            currentRoomCount++;
        }
    }

    // Destroy overlapping spawn points (doesnt work tho)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawned == false)
        {
            Destroy(gameObject);
        }
    }
}
