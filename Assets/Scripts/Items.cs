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
    public bool craftable;
    public bool hasAnimation = false;
    [SerializeField] public List<Craft> recipe = new();
    [SerializeField] public RuntimeAnimatorController animatorController;
}

public enum ItemType
{
    Consumable,
    Craftable,
}

public enum ConsumableType
{
    HealthUp,
    BigHealthUp,
    HealthDown,
    SpeedUp,
    SpeedDown,
    AttackSpeedUp,
    AttackSpeedDown,
    DamageUp,
    DamageDown,
    FireAspect,
    PoisonAspect,
    ClawHook,
    Dash,
    DoubleJump,
    Random,
    MediumHealthUp,
    None,
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

    public Craft(Items item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}