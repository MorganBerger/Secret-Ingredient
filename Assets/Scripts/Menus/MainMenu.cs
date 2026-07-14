using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SubMenu mainMenu;
    public SubMenu settingsMenu;
    public SubMenu creditsMenu;

    SubMenu currentMenu;

    Stack<SubMenu> menuHistory = new Stack<SubMenu>();

    void Start()
    {
        if (mainMenu != null) mainMenu.OnMenuButtonClicked += HandleMenuClicks;
        if (settingsMenu != null) settingsMenu.OnMenuButtonClicked += HandleMenuClicks;
        if (creditsMenu != null) creditsMenu.OnMenuButtonClicked += HandleMenuClicks;

        OpenSubMenu(mainMenu);   
    }

    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void HandleMenuClicks(string buttonTag)
    {
        switch (buttonTag)
        {
            case "playGame":
                // PlayGame("Level1Scene"); // Replace with your scene name
                break;
            case "settings":
                OpenSubMenu(settingsMenu);
                break;
            case "credits":
                OpenSubMenu(creditsMenu);
                break;
            case "back":
                GoBack();
                break;
            case "quitGame":
                QuitGame();
                break;
            default:
                Debug.LogWarning($"Unrecognized button tag clicked: {buttonTag}");
                break;
        }
    }

    void OpenSubMenu(SubMenu subMenu)
    {
        if (currentMenu != null)
        {
            menuHistory.Push(currentMenu);
            currentMenu.Hide();
        }

        currentMenu = subMenu;
        currentMenu.Show();
    }

    void GoBack()
    {
        if (menuHistory.Count == 0) return;

        currentMenu.Hide();
        
        currentMenu = menuHistory.Pop();
        currentMenu.Show();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnDestroy()
    {
        if (currentMenu != null)
        {
            currentMenu.OnMenuButtonClicked -= HandleMenuClicks;
        }

        if (mainMenu != null) mainMenu.OnMenuButtonClicked -= HandleMenuClicks;
        if (settingsMenu != null) settingsMenu.OnMenuButtonClicked -= HandleMenuClicks;
        if (creditsMenu != null) creditsMenu.OnMenuButtonClicked -= HandleMenuClicks;
    }
}