using UnityEngine;
using AYellowpaper.SerializedCollections;
using TMPro;

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