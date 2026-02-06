using UnityEngine;
using AYellowpaper.SerializedCollections;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializedDictionary("Item", "Quantity")]
    public SerializedDictionary<Items, int> items = new();
    [SerializeField] private TextMeshProUGUI itemTitleText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    public static InventoryManager Instance;

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
        itemTitleText.gameObject.SetActive(false);
        itemDescriptionText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get the current inventory in order to save it
    /// </summary>
    /// <returns>The current inventory as a SerializedDictionary of Items and their quantities</returns>
    public Dictionary<string, int> GetInventoryToSave()
    {
        Dictionary<string, int> inventoryToSave = new Dictionary<string, int>();
        foreach (var item in items)
        {
            inventoryToSave.Add(item.Key.itemName.ToString(), item.Value);
        }
        return inventoryToSave;
    }

    /// <summary>
    /// Set the inventory from the saved data
    /// </summary>
    /// <param name="savedInventory"></param>
    /// <returns>A serialized dictionnary of user's inventory</returns>
    public static SerializedDictionary<Items, int> SetInventoryFromSaveData(Dictionary<string, int> savedInventory)
    {
        SerializedDictionary<Items, int> loadedInventory = new SerializedDictionary<Items, int>();
        foreach (var entry in savedInventory)
        {
            Items item = Resources.Load<Items>($"Items/{entry.Key}");
            if (item != null)
            {
                loadedInventory.Add(item, entry.Value);
            }
            else
            {
                Debug.LogWarning($"Item '{entry.Key}' not found in Resources/Items folder.");
            }
        }
        return loadedInventory;
    }

    /// <summary>
    /// Add an item to the inventory
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <param name="quantity">The quantity to add</param>
    public void AddItem(Items item, int quantity = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        }
        else
        {
            items[item] = quantity;
        }
        FindFirstObjectByType<InventoryGrid>().RefreshInventory();
    }

    /// <summary>
    /// Remove an item from the inventory
    /// </summary>
    /// <param name="item">The item to remove</param>
    /// <param name="quantity">The quantity to remove</param>
    public void RemoveItem(Items item, int quantity = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] -= quantity;
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
        }
        FindFirstObjectByType<InventoryGrid>().RefreshInventory();
    }

    /// <summary>
    /// Show item title & description in the UI
    /// </summary>
    /// <param name="item"></param>
    public void ShowItemDescription(Items item)
    {
        itemTitleText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemTitleText.gameObject.SetActive(true);
        itemDescriptionText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide item title & description in the UI
    /// </summary>
    public void HideItemDescription()
    {
        itemTitleText.text = "";
        itemDescriptionText.text = "";
        itemTitleText.gameObject.SetActive(false);
        itemDescriptionText.gameObject.SetActive(false);
    }
}