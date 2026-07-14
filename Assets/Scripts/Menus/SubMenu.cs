using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class SubMenu : MonoBehaviour
{
    public SubMenuData data;
    public GameObject buttonPrefab;

    ButtonStack buttonStack;

    public TextMeshProUGUI titleText;

    [HideInInspector] public Action<string> OnMenuButtonClicked;

    void Start()
    {
        buttonStack = GetComponentInChildren<ButtonStack>();

        titleText.text = data.title;

        GenerateButtons();
        buttonStack.Ready();
    }

    public void GenerateButtons()
    {
        for (int i = buttonStack.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(buttonStack.transform.GetChild(i).gameObject);
        }

        if (data == null || data.buttons == null) return;

        foreach (var buttonData in data.buttons)
        {
            GameObject newButtonObj = Instantiate(buttonPrefab, buttonStack.gameObject.transform);

            SubMenuButton buttonScript = newButtonObj.GetComponent<SubMenuButton>();
            buttonScript.Initialize(buttonData);

            Button btn = newButtonObj.GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    OnMenuButtonClicked?.Invoke(buttonData.tag);
                });
            }
        }
    }

    void Update()
    {
        if (InputSystem.actions["Cancel"].WasPressedThisFrame()) 
        {
            OnMenuButtonClicked?.Invoke("back");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
