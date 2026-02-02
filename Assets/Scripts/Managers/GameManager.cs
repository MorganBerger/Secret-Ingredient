using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    private bool gamePaused = false;
    public int currentLevel = 1;
    private Character character;
    private SaveData saveData;
    public int currentSeed;

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

        saveData = SaveManager.LoadGame();
    }

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        LoadDataInTheGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseGame();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            SaveData data = new()
            {
                level = currentLevel,
                seed = currentSeed,
                positionX = character.transform.position.x,
                positionY = character.transform.position.y,
                positionZ = character.transform.position.z,
                currentHealth = character.health,
                speed = character.speed,
                attackSpeed = character.attackSpeed,
                damage = character.damage,
                inventory = InventoryManager.Instance.GetInventoryToSave(),
                canDash = CharacterSkills.canDash,
                canDoubleJump = CharacterSkills.canDoubleJump,
                canWallClimb = CharacterSkills.canWallClimb,
            };
            SaveManager.SaveGame(data);
        }
    }

    private void LoadDataInTheGame()
    {
        currentLevel = saveData.level;
        currentSeed = saveData.seed;
        character.transform.position = new Vector3(saveData.positionX, saveData.positionY, saveData.positionZ);
        character.health = saveData.currentHealth;
        character.speed = saveData.speed;
        character.attackSpeed = saveData.attackSpeed;
        character.damage = saveData.damage;
        InventoryManager.Instance.items = InventoryManager.SetInventoryFromSaveData(saveData.inventory);
        CharacterSkills.canDash = saveData.canDash;
        CharacterSkills.canDoubleJump = saveData.canDoubleJump;
        CharacterSkills.canWallClimb = saveData.canWallClimb;
    }

    private void TogglePauseGame()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0f : 1f;
    }
}

[System.Serializable]
public class SerializableKeyValuePair
{
    public string key;
    public int value;
}