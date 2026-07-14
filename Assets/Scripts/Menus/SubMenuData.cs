using UnityEngine;

[CreateAssetMenu(fileName = "NewSubMenu", menuName = "Game Data/Sub Menu")]
public class SubMenuData : ScriptableObject
{
    public string title;  
    
    [Header("Buttons")]
    public SubMenuButtonData[] buttons;  
}

[CreateAssetMenu(fileName = "NewSubMenuButton", menuName = "Game Data/Sub Menu Button")]
public class SubMenuButtonData : ScriptableObject
{
    [Header("Buttons")]
    public string title;
    public string tag;
}