using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class ButtonStack : MonoBehaviour
{    
    [HideInInspector] public List<Button> buttons = new List<Button>();
    
    public void Ready()
    {
        buttons = GetComponentsInChildren<Button>().ToList();

        if (buttons.Count > 0)
        {
            Debug.Log($"ButtonStack: Ready with {buttons.Count} buttons. Selecting the first button.");
            buttons[0].Select();
        }
    }
}
