using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool isMenuOpened = false;
    [SerializeField] private GameObject mainMenu;
    private CanvasGroup canvasGroup;
    private readonly float fadeDuration = 0.25f;
    private Coroutine currentFade;
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
        if (mainMenu != null)
        {
            canvasGroup = mainMenu.GetComponent<CanvasGroup>();
        } else
        {
            Debug.LogWarning("Main menu has not been assigned yet");
        }
        
        // Cache le menu au démarrage
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            if (isMenuOpened)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }
    }

    public void ShowMenu()
    {
        isMenuOpened = true;
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(FadeIn());
    }

    public void HideMenu()
    {
        if (isMenuOpened == false) return;

        isMenuOpened = false;
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}