using UnityEngine;
using TMPro;

public class SubMenuButton : MonoBehaviour
{
    SubMenuButtonData data;

    public string eventName;

    public void Initialize(SubMenuButtonData data)
    {
        this.data = data;
        this.eventName = data.tag;

        var textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null && data != null)
        {
            textComponent.text = data.title;
        }
    }
}
