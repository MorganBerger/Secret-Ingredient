using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject craftMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private CanvasGroup craftMenuCG;
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
                ShowMenu(craftMenu, craftMenuCG);
            }
        }
    }

    private void ShowMenu(GameObject menu, CanvasGroup cg)
    {
        if (menuStates[menu] == true) return;
        menuStates[menu] = true;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn(cg));
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

    public void ShowCraftMenu()
    {
        ShowMenu(craftMenu, craftMenuCG);
    }

    public void HideCraftMenu()
    {
        HideMenu(craftMenu, craftMenuCG);
    }
}