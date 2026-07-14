using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

[ExecuteAlways]
public class ButtonStack : MonoBehaviour
{    
    [SerializeField] public List<Button> buttons = new List<Button>();

    public void SelectFirst()
    {
        if (buttons.Count > 0)
        {
            buttons[0].Select();
        }
    }

    public void Ready()
    {
        buttons = GetComponentsInChildren<Button>().ToList();

        SelectFirst();
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            
            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }

            buttons.Remove(child.GetComponent<Button>());
        }
    }
}
