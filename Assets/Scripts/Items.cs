using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Secret Ingredient/Item")]
public class Items : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public ItemType itemType;
    public ConsumableType consumableType;
    public ItemRarity itemRarity;
    [SerializeField] private List<Craft> recipe = new();
}

public enum ItemType
{
    Consumable,
    Equipment,
    Power,
}

public enum ConsumableType
{
    HealthUp,
    MediumHealthUp,
    HealthDown,
    SpeedUp,
    SpeedDown,
    AttackSpeedUp,
    AttackSpeedDown,
    DamageUp,
    DamageDown,
    Power,
    FireAspect,
    Range,
}

public enum ItemRarity
{
    Common,
    Rare,
    Mythic,
}

[System.Serializable]
public class Craft
{
    public Items item;
    public int quantity;
}