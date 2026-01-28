using UnityEngine;
using System.Collections.Generic;

public enum DoorDirection
{
    None,
    Top,
    Right,
    Bottom,
    Left
}

[System.Serializable]
public class RoomPrefab
{
    [Header("Room Configuration Prefab")]
    public GameObject prefab;
    [Space(5)]

    [Header("Available Doors")]
    public bool hasTopDoor;
    public bool hasRightDoor;
    public bool hasBottomDoor;
    public bool hasLeftDoor;

    /// <summary>
    /// Check if the room has a door in the specified direction
    /// </summary>
    /// <param name="direction">The direction to check for a door</param>
    /// <returns>True if the room has a door in the specified direction, otherwise false</returns>
    public bool HasDoor(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.Top => hasTopDoor,
            DoorDirection.Right => hasRightDoor,
            DoorDirection.Bottom => hasBottomDoor,
            DoorDirection.Left => hasLeftDoor,
            _ => false
        };
    }

    /// <summary>
    /// Get a list of all door directions available in this room
    /// </summary>
    /// <returns>A list of door directions</returns>
    public List<DoorDirection> GetAllDoors()
    {
        List<DoorDirection> doors = new();
        if (hasTopDoor) doors.Add(DoorDirection.Top);
        if (hasRightDoor) doors.Add(DoorDirection.Right);
        if (hasBottomDoor) doors.Add(DoorDirection.Bottom);
        if (hasLeftDoor) doors.Add(DoorDirection.Left);
        return doors;
    }

    /// <summary>
    /// Get the total number of doors in this room
    /// </summary>
    /// <returns>The number of doors</returns>
    public int DoorCount()
    {
        int count = 0;
        if (hasTopDoor) count++;
        if (hasRightDoor) count++;
        if (hasBottomDoor) count++;
        if (hasLeftDoor) count++;
        return count;
    }
}

public class RoomInstance
{
    public Vector2Int gridPosition;
    public RoomPrefab roomPrefab;
    public GameObject instantiatedObject;
    
    public RoomInstance(Vector2Int pos, RoomPrefab prefab)
    {
        gridPosition = pos;
        roomPrefab = prefab;
    }
}