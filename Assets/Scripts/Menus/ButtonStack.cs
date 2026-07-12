using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class ButtonStack : MonoBehaviour
{    
    int currentIndex = 0;

    List<Button> buttons = new List<Button>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttons = GetComponentsInChildren<Button>().ToList();
        buttons[0].Select();
    }
}
