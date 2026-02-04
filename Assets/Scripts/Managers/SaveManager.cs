using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class SaveManager: MonoBehaviour
{
    public static SaveManager Instance;
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SaveGame(SaveData data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SavePath, json);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
        return new SaveData();
    }
}

[System.Serializable]
public class SaveData
{
    public int level;
    public int seed;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float currentHealth;
    public float speed;
    public float attackSpeed;
    public float damage;
    public Dictionary<string, int> inventory;
    public bool canDash;
    public bool canDoubleJump;
    public bool canWallClimb;
}