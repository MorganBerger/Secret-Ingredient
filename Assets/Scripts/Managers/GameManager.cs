using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    private bool gamePaused = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseGame();
        }
    }

    private void TogglePauseGame()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0f : 1f;
    }
}