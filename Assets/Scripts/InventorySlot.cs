using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    private Items currentItem;
    private int quantity;
    private DropZone leftCraftZone;
    private DropZone rightCraftZone;
    private bool isDragging = false;

    void Awake()
    {
        leftCraftZone = GameObject.Find("LeftCraftSlot").GetComponent<DropZone>();
        rightCraftZone = GameObject.Find("RightCraftSlot").GetComponent<DropZone>();
    }

    /// <summary>
    /// Set the item and quantity for this slot
    /// </summary>
    /// <param name="item">The scriptable object representing the item</param>
    /// <param name="quantity">The quantity of the item</param>
    public void SetItem(Items item, int quantity)
    {
        currentItem = item;
        this.quantity = quantity;

        if (item != null)
        {
            itemIcon.sprite = item.itemSprite;
            itemIcon.enabled = true;

            if (quantity > 0)
            {
                quantityText.text = quantity.ToString();
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }
        else
        {
            ClearSlot();
        }
    }

    /// <summary>
    /// Clear the inventory slot by removing every visuals
    /// </summary>
    public void ClearSlot()
    {
        currentItem = null;
        quantity = 0;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        quantityText.enabled = false;
    }

    public Items GetItem() => currentItem;
    public bool HasItem() => currentItem != null;

    /// <summary>
    /// Handle begin drag event
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!HasItem()) return;

        isDragging = true;
        DragVisualManager.Instance.StartDrag(currentItem.itemSprite);
    }

    /// <summary>
    /// Handle drag event
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (!HasItem() || !isDragging) return;

        DragVisualManager.Instance.UpdateDragPosition(eventData.position);
    }

    /// <summary>
    /// Handle end drag event
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;
        DragVisualManager.Instance.EndDrag();
    }

    /// <summary>
    /// Easier way to add item to craft slot on click with CMD/Ctrl held
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasItem()) return;

        // Detect CMD/Ctrl keys
        bool isCommandHeld = Input.GetKey(KeyCode.LeftCommand) ||
                            Input.GetKey(KeyCode.RightCommand) ||
                            Input.GetKey(KeyCode.LeftControl) ||
                            Input.GetKey(KeyCode.RightControl);

        if (isCommandHeld)
        {
            DropZone availableCraftSlot = leftCraftZone.HasItem() ? rightCraftZone : leftCraftZone;
            if (availableCraftSlot != null)
            {
                if (availableCraftSlot.HasItem())
                {
                    // If the craft slot already has an item, return it to inventory
                    InventoryManager.Instance.AddItem(availableCraftSlot.GetItem(), 1);
                }
                availableCraftSlot.ReceiveItem(currentItem);
                InventoryManager.Instance.RemoveItem(currentItem, 1);
                FindFirstObjectByType<InventoryGrid>().RefreshInventory();
            }
        }
    }
}
