using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject craftMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private CanvasGroup craftMenuCG;
    [SerializeField] private CanvasGroup deathMenuCG;
    [SerializeField] private CanvasGroup inventoryMenuCG;
    private Dictionary<GameObject, bool> menuStates = new();
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
        if (craftMenu != null)
        {
            craftMenuCG = craftMenu.GetComponent<CanvasGroup>();
            menuStates[craftMenu] = false;
        } else
        {
            Debug.LogWarning("Craft menu has not been assigned yet");
        }

        if (inventoryMenu != null)
        {
            inventoryMenuCG = inventoryMenu.GetComponent<CanvasGroup>();
            menuStates[inventoryMenu] = false;
        } else
        {
            Debug.LogWarning("Inventory menu has not been assigned yet");
        }

        if (deathMenu != null)
        {
            deathMenuCG = deathMenu.GetComponent<CanvasGroup>();
            menuStates[deathMenu] = false;
        } else
        {
            Debug.LogWarning("Death menu has not been assigned yet");
        }

        // Ensure all menus are hidden at the start
        foreach (KeyValuePair<GameObject, bool> kvp in menuStates)
        {
            ResetMenu(kvp.Key.GetComponent<CanvasGroup>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            List<GameObject> keys = new(menuStates.Keys);
            foreach (GameObject key in keys) {
                // menuStates[key] = false;
                HideMenu(key, key.GetComponent<CanvasGroup>());
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            if (menuStates[inventoryMenu] == true)
            {
                HideMenu(inventoryMenu, inventoryMenuCG);
            }
            else
            {
                ShowMenu(inventoryMenu, inventoryMenuCG);
            }
        }
    }

    private void ShowMenu(GameObject menu, CanvasGroup cg)
    {
        if (menuStates[menu] == true) return;
        CloseEveryMenus();
        menuStates[menu] = true;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn(cg));
    }

    public void CloseEveryMenus()
    {
        List<GameObject> keys = new(menuStates.Keys);
        foreach (GameObject key in keys) {
            menuStates[key] = false;
            HideMenu(key, key.GetComponent<CanvasGroup>());
        }
    }

    private void HideMenu(GameObject menu, CanvasGroup cg)
    {
        if (menuStates[menu] == false) return;

        menuStates[menu] = false;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut(cg));
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
        if (menuStates[craftMenu] == true)
        {
            HideMenu(craftMenu, craftMenuCG);
        }
        else
        {
            ShowMenu(craftMenu, craftMenuCG);
        }
    }

    public void HideCraftMenu()
    {
        HideMenu(craftMenu, craftMenuCG);
    }

    public void ShowDeathMenu()
    {
        ShowMenu(deathMenu, deathMenuCG);
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