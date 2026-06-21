using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup craftMenuCG;
    [SerializeField] private CanvasGroup deathMenuCG;
    [SerializeField] private CanvasGroup inventoryMenuCG;
    [SerializeField] private CanvasGroup blackScreenCG;
    private readonly Dictionary<CanvasGroup, bool> menuStates = new();
    private readonly float fadeDuration = 0.25f;
    private Coroutine fadeCoroutine;
    public static UIManager Instance;

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

    private void Start()
    {
        if (craftMenuCG != null) menuStates[craftMenuCG] = false;
        else Debug.LogWarning("Craft menu has not been assigned yet");

        if (inventoryMenuCG != null) menuStates[inventoryMenuCG] = false;
        else Debug.LogWarning("Inventory menu has not been assigned yet");


        if (deathMenuCG != null) menuStates[deathMenuCG] = false;
        else Debug.LogWarning("Death menu has not been assigned yet");

        if (blackScreenCG != null) menuStates[blackScreenCG] = false;
        else Debug.LogWarning("Black screen has not been assigned yet");


        // Ensure all menus are hidden at the start
        foreach (KeyValuePair<CanvasGroup, bool> kvp in menuStates)
        {
            ResetMenu(kvp.Key);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            List<CanvasGroup> keys = new(menuStates.Keys);
            foreach (CanvasGroup key in keys) {
                HideMenu(key);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            if (menuStates[inventoryMenuCG] == true)
            {
                HideMenu(inventoryMenuCG);
            }
            else
            {
                ShowMenu(inventoryMenuCG);
            }
        }
    }

    private void ShowMenu(CanvasGroup menu)
    {
        if (menuStates[menu] == true) return;
        CloseEveryMenus();
        menuStates[menu] = true;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn(menu));
    }

    public void CloseEveryMenus()
    {
        List<CanvasGroup> keys = new(menuStates.Keys);
        foreach (CanvasGroup key in keys) {
            HideMenu(key);
        }
    }

    private void HideMenu(CanvasGroup menu)
    {
        if (menuStates[menu] == false) return;

        menuStates[menu] = false;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut(menu));
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        cg.interactable = true;
        cg.blocksRaycasts = true;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup cg)
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
    }

     private void ResetMenu(CanvasGroup cg)
    {
        if (cg != null)
        {
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void ToggleCraftMenu()
    {
        if (menuStates[craftMenuCG] == true)
        {
            HideMenu(craftMenuCG);
        }
        else
        {
            ShowMenu(craftMenuCG);
        }
    }

    public void HideCraftMenu()
    {
        HideMenu(craftMenuCG);
    }

    public void ShowDeathMenu()
    {
        ShowMenu(deathMenuCG);
    }

    public void ShowBlackScreen()
    {
        ShowMenu(blackScreenCG);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    /// <summary>
    /// Reloads the current scene, reloading data
    /// </summary>
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}