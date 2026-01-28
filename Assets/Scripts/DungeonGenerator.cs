using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    [SerializeField] private List<RoomPrefab> roomPrefabs = new();

    [Header("Generation Settings")]
    [SerializeField] private int targetCoreRooms = 8;
    [SerializeField] private float roomSize = 6f;
    [SerializeField] private int maxAttempts = 100;
    [SerializeField] private bool generateOnStart = true;

    private readonly Dictionary<Vector2Int, RoomInstance> grid = new();
    private readonly Queue<Vector2Int> roomsToExpand = new();
    private System.Random random;

    void Start()
    {
        if (generateOnStart)
        {
            GenerateDungeon();
        }
    }

    /// <summary>
    /// Generate a procedural dungeon layout
    /// </summary>
    /// <param name="seed">Optional seed for random generation</param>
    public void GenerateDungeon(int? seed = null)
    {
        // Ensure no previous dungeon exists
        ClearDungeon();

        random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();

        // Get a first room to start the generation
        RoomPrefab startRoom = GetRandomStartRoom();
        if (startRoom == null)
        {
            Debug.LogWarning("No room available to start the generation!");
            return;
        }

        Vector2Int startPos = Vector2Int.zero;
        PlaceRoom(startPos, startRoom);
        roomsToExpand.Enqueue(startPos);

        // Generate the following rooms until we reach the target or attempt limit
        int attempts = 0;
        while (grid.Count < targetCoreRooms && roomsToExpand.Count > 0 && attempts < maxAttempts)
        {
            attempts++;

            Vector2Int currentPos = roomsToExpand.Dequeue();
            RoomInstance currentRoom = grid[currentPos];
            TryExpandRoom(currentPos, currentRoom);
        }

        // Close potentially open exists
        List<Vector2Int> remainingPositionsToFill = GetRemainingPositionsToFill();
        CloseOpenExits(remainingPositionsToFill);

        Debug.Log($"Dungeon generated with {grid.Count} rooms in {attempts} attempts");
    }

    /// <summary>
    /// Get all rooms that still have open exits to fill
    /// </summary>
    /// <returns>A list of vector2 positions</returns>
    public List<Vector2Int> GetRemainingPositionsToFill()
    {
        List<Vector2Int> positionsToFill = new();

        foreach (KeyValuePair<Vector2Int, RoomInstance> kvp in grid)
        {
            Vector2Int position = kvp.Key;
            RoomInstance room = kvp.Value;

            foreach (DoorDirection direction in room.roomPrefab.GetAllDoors())
            {
                Vector2Int adjacentPos = GetAdjacentPosition(position, direction);

                if (!grid.ContainsKey(adjacentPos) && !positionsToFill.Contains(adjacentPos))
                {
                    positionsToFill.Add(adjacentPos);
                }
            }
        }
        return positionsToFill;
    }

    /// <summary>
    /// Close all open exits by placing compatible rooms with dead-ends where possible
    /// </summary>
    /// <param name="positionsToFill">A list of positions with no dead-ends</param>
    public void CloseOpenExits(List<Vector2Int> positionsToFill)
    {
        if (positionsToFill.Count > 0) {
            Debug.Log($"{positionsToFill.Count} positions need adjacent rooms to close open exits.");

            int closedExits = 0;
            foreach (Vector2Int position in positionsToFill)
            {
                List<RoomPrefab> compatibleRooms = GetCompatibleRooms(position, DoorDirection.None);
                List<RoomPrefab> deadEndRooms = compatibleRooms.Where(r => r.DoorCount() == 1).ToList();
                RoomPrefab selectedRoom = null;

                // Prefer dead-ends if available
                if (deadEndRooms.Count > 0)
                {
                    selectedRoom = deadEndRooms[random.Next(deadEndRooms.Count)];
                }
                else if (compatibleRooms.Count > 0)
                {
                    selectedRoom = compatibleRooms[random.Next(compatibleRooms.Count)];
                }

                // Place found compatible room
                if (selectedRoom != null)
                {
                    PlaceRoom(position, selectedRoom);
                    closedExits++;
                }
                else
                {
                    Debug.LogWarning($"Impossible to close exit at position {position} - no compatible room found!");
                }
            }
            Debug.Log($"{closedExits} exits successfully closed");

            List<Vector2Int> stillOpenedPositions = GetRemainingPositionsToFill();
            CloseOpenExits(stillOpenedPositions);
        }
    }

    /// <summary>
    /// Try to expand the dungeon from the given room in all valid directions
    /// </summary>
    /// <param name="position">Current room position to expand from</param>
    /// <param name="room">Current room instance to expand from</param>
    private void TryExpandRoom(Vector2Int position, RoomInstance room)
    {
        List<DoorDirection> directions = new()
        { 
            DoorDirection.Top,
            DoorDirection.Right,
            DoorDirection.Bottom,
            DoorDirection.Left,
        };

        // Randomize directions order
        directions = directions.OrderBy(x => random.Next()).ToList();

        foreach (DoorDirection direction in directions)
        {
            // If the current room has a door in this direction
            if (!room.roomPrefab.HasDoor(direction))
                continue;

            // And if adjacent position is free
            Vector2Int newPos = GetAdjacentPosition(position, direction);
            if (grid.ContainsKey(newPos)) {
                continue;
            }

            // Find and place a compatible room
            DoorDirection requiredDoor = GetOppositeDoor(direction);
            List<RoomPrefab> compatibleRooms = GetCompatibleRooms(newPos, requiredDoor);
            RoomPrefab selectedRoom = compatibleRooms[random.Next(compatibleRooms.Count)];
            if (selectedRoom != null)
            {
                PlaceRoom(newPos, selectedRoom);
                roomsToExpand.Enqueue(newPos);
            }
        }
    }

    /// <summary>
    /// Get a compatible room, given the current position's adjacent constraints
    /// </summary>
    /// <param name="position">Current room position</param>
    /// <param name="requiredDoor">The door direction that must be present in the compatible room</param>
    /// <returns></returns>
    private List<RoomPrefab> GetCompatibleRooms(Vector2Int position, DoorDirection requiredDoor)
    {
        // Retrieve all constraints based on adjacent rooms
        bool mustHaveTop = false, mustHaveRight = false, mustHaveBottom = false, mustHaveLeft = false;
        bool cannotHaveTop = false, cannotHaveRight = false, cannotHaveBottom = false, cannotHaveLeft = false;

        // Set the mandatory door
        switch (requiredDoor)
        {
            case DoorDirection.Top: mustHaveTop = true; break;
            case DoorDirection.Right: mustHaveRight = true; break;
            case DoorDirection.Bottom: mustHaveBottom = true; break;
            case DoorDirection.Left: mustHaveLeft = true; break;
        }

        // Check each direction for constraints
        CheckAdjacentConstraint(position, DoorDirection.Top, ref mustHaveTop, ref cannotHaveTop);
        CheckAdjacentConstraint(position, DoorDirection.Right, ref mustHaveRight, ref cannotHaveRight);
        CheckAdjacentConstraint(position, DoorDirection.Bottom, ref mustHaveBottom, ref cannotHaveBottom);
        CheckAdjacentConstraint(position, DoorDirection.Left, ref mustHaveLeft, ref cannotHaveLeft);

        // Filter compatible rooms
        List<RoomPrefab> compatibleRooms = new List<RoomPrefab>();

        foreach (var room in roomPrefabs)
        {
            // Check mandatory doors
            if (mustHaveTop && !room.hasTopDoor) continue;
            if (mustHaveRight && !room.hasRightDoor) continue;
            if (mustHaveBottom && !room.hasBottomDoor) continue;
            if (mustHaveLeft && !room.hasLeftDoor) continue;

            // Check forbidden doors
            if (cannotHaveTop && room.hasTopDoor) continue;
            if (cannotHaveRight && room.hasRightDoor) continue;
            if (cannotHaveBottom && room.hasBottomDoor) continue;
            if (cannotHaveLeft && room.hasLeftDoor) continue;

            compatibleRooms.Add(room);
        }

        if (compatibleRooms.Count == 0)
            return null;

        // Return a random room among the compatible ones
        return compatibleRooms;
    }

    /// <summary>
    /// Check adjacent available rooms to set current position's constraints
    /// </summary>
    /// <param name="position">Current room position</param>
    /// <param name="direction">Direction to check</param>
    /// <param name="mustHave">Ref to possible mandatory door</param>
    /// <param name="cannotHave">Ref to possible forbidden door</param>
    private void CheckAdjacentConstraint(Vector2Int position, DoorDirection direction, 
        ref bool mustHave, ref bool cannotHave)
    {
        Vector2Int adjacentPos = GetAdjacentPosition(position, direction);

        if (grid.ContainsKey(adjacentPos))
        {
            // There already is an adjacent room
            RoomInstance adjacentRoom = grid[adjacentPos];
            DoorDirection oppositeDir = GetOppositeDoor(direction);

            if (adjacentRoom.roomPrefab.HasDoor(oppositeDir))
            {
                // The adjacent room has a door towards the current one
                // Set a mandatory rule
                mustHave = true;
            }
            else
            {
                // The adjacent room doesn't have a door towards the current one
                // Set a forbidden rule
                cannotHave = true;
            }
        }
    }

    /// <summary>
    /// Instantiate and register a room at the given grid position
    /// </summary>
    /// <param name="position">Grid position to place the room</param>
    /// <param name="roomPrefab">RoomPrefab to instantiate</param>
    private void PlaceRoom(Vector2Int position, RoomPrefab roomPrefab)
    {
        RoomInstance roomInstance = new(position, roomPrefab);

        // Instantiate prefab as a game object
        Vector3 worldPosition = new(position.x * roomSize, position.y * roomSize, 0);
        roomInstance.instantiatedObject = Instantiate(roomPrefab.prefab, worldPosition, Quaternion.identity, transform);
        roomInstance.instantiatedObject.name = $"Room_{position.x}_{position.y}";

        // Register the new instance into the grid
        grid[position] = roomInstance;
    }

    /// <summary>
    /// Get a random room to start the dungeon generation
    /// </summary>
    /// <returns>A possible room prefab</returns>
    private RoomPrefab GetRandomStartRoom()
    {
        // Prefer rooms with multiple doors for the start
        var multiDoorRooms = roomPrefabs.Where(r => r.DoorCount() > 1).ToList();

        if (multiDoorRooms.Count > 0)
            return multiDoorRooms[random.Next(multiDoorRooms.Count)];

        if (roomPrefabs.Count > 0)
            return roomPrefabs[random.Next(roomPrefabs.Count)];

        return null;
    }

    /// <summary>
    /// Get the adjacent grid position based on a direction
    /// </summary>
    /// <param name="position">Current position</param>
    /// <param name="direction">Direction to get adjacent position</param>
    /// <returns>Adjacent grid position</returns>
    private Vector2Int GetAdjacentPosition(Vector2Int position, DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.Top => position + Vector2Int.up,
            DoorDirection.Right => position + Vector2Int.right,
            DoorDirection.Bottom => position + Vector2Int.down,
            DoorDirection.Left => position + Vector2Int.left,
            _ => position
        };
    }

    /// <summary>
    /// Get the opposite door direction
    /// </summary>
    /// <param name="direction">The current door direction</param>
    /// <returns>The opposite door direction</returns>
    private DoorDirection GetOppositeDoor(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.Top => DoorDirection.Bottom,
            DoorDirection.Right => DoorDirection.Left,
            DoorDirection.Bottom => DoorDirection.Top,
            DoorDirection.Left => DoorDirection.Right,
            _ => DoorDirection.None
        };
    }

    /// <summary>
    /// Editor method to clear generated level
    /// </summary>
    public void ClearDungeon()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        foreach (var child in children)
        {
            DestroyImmediate(child);
        }

        grid.Clear();
        roomsToExpand.Clear();
    }
}