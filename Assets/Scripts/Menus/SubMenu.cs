using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class SubMenu : MonoBehaviour
{
    public SubMenuData data;
    public GameObject buttonPrefab;

    ButtonStack buttonStack;

    public TextMeshProUGUI titleText;

    [HideInInspector] public Action<string> OnMenuButtonClicked;

    void Awake()
    {
        buttonStack = GetComponentInChildren<ButtonStack>();

        titleText.text = data.title;

        GenerateButtons();

        if (Application.isPlaying)
        {
            buttonStack.Ready();
        }
    }

    public void GenerateButtons()
    {
        if (buttonStack == null) return;

        buttonStack.Clear();

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
        if (!Application.isPlaying) return;

        if (InputSystem.actions["Navigate"].WasPressedThisFrame())
        {
            bool somethingSelected = EventSystem.current.currentSelectedGameObject != null;

            Debug.Log("Navigate input detected. Something selected: " + somethingSelected);

            if (!somethingSelected)
            {
                buttonStack.SelectFirst();
            }
        } 

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
