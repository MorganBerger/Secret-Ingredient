using UnityEngine;
using System.Collections.Generic;

public class InventoryGrid : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform contentParent;
    [SerializeField] private bool shouldEnablePointerEvents = true;

    private List<InventorySlot> slots = new();
    private int maxSlots = 24;
    private bool shadowRefreshed = false;

    void Start()
    {
        CreateSlots();
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.RefreshInventories();
    }

    void Update()
    {
        // Little cheat code here, update once potion shadows
        if (!shadowRefreshed)
        {
            InventoryManager.Instance.RefreshInventories();
            shadowRefreshed = true;
        }
    }

    /// <summary>
    /// Create inventory slots based on the maximum inventory size
    /// </summary>
    void CreateSlots()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, contentParent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slot.shouldEnablePointerEvents = shouldEnablePointerEvents;
            slots.Add(slot);
        }
    }

    /// <summary>
    /// Refresh the inventory UI to reflect updated current items list
    /// </summary>
    public void RefreshInventory()
    {
        // first empty every slots just in case
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();
        }

        int index = 0;
        foreach (KeyValuePair<Items, int> kvp in InventoryManager.Instance.items)
        {
            // Fill the slots with current possessed items
            if (index < slots.Count)
            {
                slots[index].SetItem(kvp.Key, kvp.Value);
                index++;
            }
        }
    }
}