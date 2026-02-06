using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image itemIcon;
    [SerializeField] private Image itemPlaceholder;
    [SerializeField] private Image crossButton;
    private Items currentItem = null;

    public void Start()
    {
        if (itemPlaceholder != null)
        {
            itemPlaceholder.enabled = true;
        }

        if (itemIcon != null)
        {
            itemIcon.enabled = false;
        }

        if (crossButton != null)
        {
            crossButton.enabled = false;
        }

        currentItem = null;
    }

    public void Update()
    {
        itemPlaceholder.enabled = !HasItem();
        itemIcon.enabled = HasItem();
        crossButton.enabled = HasItem();
    }

    /// <summary>
    /// Drop handler to receive item from inventory slot
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (CraftManager.Instance.IsCraftDisabled()) return;

        InventorySlot draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (draggedSlot != null && draggedSlot.HasItem())
        {
            if (HasItem())
            {
                // If the drop zone already has an item, return it to inventory
                InventoryManager.Instance.AddItem(currentItem, 1);
            }
            ReceiveItem(draggedSlot.GetItem());
            InventoryManager.Instance.RemoveItem(draggedSlot.GetItem(), 1);
            FindFirstObjectByType<InventoryGrid>().RefreshInventory();
            CraftManager.Instance.UpdateCraftStatus();
        }
    }

    /// <summary>
    /// Receive an item into the craft slot and manage visuals
    /// </summary>
    /// <param name="item"></param>
    public void ReceiveItem(Items item)
    {
        currentItem = item;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.itemSprite;
            itemIcon.preserveAspect = true;
        }
        CraftManager.Instance.UpdateCraftStatus();
    }

    /// <summary>
    /// Clear method to empty the craft slot and manage visuals
    /// </summary>
    public void ClearZone()
    {
        currentItem = null;
        if (itemIcon != null) itemIcon.enabled = false;
        if (itemPlaceholder != null) itemPlaceholder.enabled = true;
        if (crossButton != null) crossButton.enabled = false;
        CraftManager.Instance.UpdateCraftStatus();
    }

    /// <summary>
    /// Check if craft zone has already an item
    /// </summary>
    /// <returns></returns>
    public bool HasItem()
    {
        return currentItem != null;
    }

    /// <summary>
    /// Helper to get current item in the craft slot
    /// </summary>
    /// <returns></returns>
    public Items GetItem()
    {
        return currentItem;
    }

    /// <summary>
    /// Clear craft slot on right click
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasItem() || CraftManager.Instance.isCrafting) return;
        if (eventData.button == PointerEventData.InputButton.Right) {
            InventoryManager.Instance.AddItem(currentItem, 1);
            ClearZone();
        }
    }

    /// <summary>
    /// Add back item to inventory when clear button is clicked
    /// </summary>
    public void OnCrossClear()
    {
        if (!HasItem() || CraftManager.Instance.isCrafting) return;
        InventoryManager.Instance.AddItem(currentItem, 1);
        ClearZone();
    }

    /// <summary>
    /// Show item description on pointer enter
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasItem())
        {
            CanvasGroup panel = GetComponentInParent<CanvasGroup>();
            InventoryManager.Instance.ShowItemDescription(panel, currentItem);
        }
    }

    /// <summary>
    /// Hide item description on pointer exit
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (HasItem())
        {
            CanvasGroup panel = GetComponentInParent<CanvasGroup>();
            InventoryManager.Instance.HideItemDescription(panel);
        }
    }
}