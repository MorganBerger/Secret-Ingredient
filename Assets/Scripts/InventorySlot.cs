using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    [SerializeField] private Image consumableHintImg;
    private Items currentItem;
    private int quantity;
    private DropZone leftCraftZone;
    private DropZone rightCraftZone;
    private bool isDragging = false;
    private bool disabled = false;
    private Animator animator;
    public float doubleClickTime = 0.3f;
    private float lastClickTime = 0f;
    private int currentClickCount = 0;

    void Awake()
    {
        leftCraftZone = GameObject.Find("LeftCraftSlot").GetComponent<DropZone>();
        rightCraftZone = GameObject.Find("RightCraftSlot").GetComponent<DropZone>();
        animator = itemIcon.gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = itemIcon.gameObject.AddComponent<Animator>();
        }
    }

    private void Start()
    {
        consumableHintImg.gameObject.SetActive(true);
        consumableHintImg.enabled = false;
    } 

    void Update()
    {
        disabled = CraftManager.Instance.IsCraftDisabled();
        itemIcon.color = disabled ? new Color(1f, 1f, 1f, 0.5f) : Color.white;
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
            itemIcon.preserveAspect = true;
            itemIcon.enabled = true;
            Debug.Log("Setting item in slot: " + item.itemName + " x" + quantity + "and type " + item.itemType);
            consumableHintImg.enabled = item.itemType == ItemType.Consumable;
            Debug.Log("Consumable hint enabled: " + consumableHintImg.enabled);
            if (item.animatorController != null && item.hasAnimation)
            {
                animator.runtimeAnimatorController = item.animatorController;
                animator.enabled = true;
                animator.Play("Idle");
            }
            else
            {
                animator.runtimeAnimatorController = null;
                animator.enabled = false;
            }

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
            consumableHintImg.enabled = false;
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
        consumableHintImg.enabled = false;
    }

    public Items GetItem() => currentItem;
    public bool HasItem() => currentItem != null;

    /// <summary>
    /// Handle begin drag event
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!HasItem() || CraftManager.Instance.IsCraftDisabled() || !currentItem.craftable) return;

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
        currentClickCount++;

        // Detect CMD/Ctrl keys
        bool isCommandHeld = Input.GetKey(KeyCode.LeftCommand) ||
                            Input.GetKey(KeyCode.RightCommand) ||
                            Input.GetKey(KeyCode.LeftControl) ||
                            Input.GetKey(KeyCode.RightControl);

        if (isCommandHeld)
        {
            if (CraftManager.Instance.IsCraftDisabled() || !currentItem.craftable) return;

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
            currentClickCount = 0;
            return;
        }

        // Handle double click to consume the item
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= doubleClickTime)
        {
            if (currentItem.itemType == ItemType.Consumable)
            {
                UIManager.Instance.HideMenu();
                Character playerCharacter = FindFirstObjectByType<Character>();
                playerCharacter.ConsumeItem(currentItem);
            }
            lastClickTime = 0f;
            currentClickCount = 0;
        } else
        {
            lastClickTime = Time.time;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasItem())
        {
            InventoryManager.Instance.ShowItemDescription(currentItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HasItem())
        {
            InventoryManager.Instance.HideItemDescription();
        }
    }
}