using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftResult : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Items item;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Image pickIcon;
    private Animator animator;

    public void Start()
    {
        if (pickIcon != null)
        {
            pickIcon.enabled = false;
        }
        animator = ItemImage.gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = ItemImage.gameObject.AddComponent<Animator>();
        }
    }

    public void Update()
    {
        pickIcon.enabled = item != null;
    }

    /// <summary>
    /// Sets the item to be displayed in the craft result
    /// </summary>
    /// <param name="item">The item data</param>
    public void SetItem(Items item)
    {
        this.item = item;
        if (item != null && item.animatorController != null && item.hasAnimation)
        {
            animator.runtimeAnimatorController = item.animatorController;
            animator.enabled = true;
            animator.Play("Idle");
        }
    }

    /// <summary>
    /// Adds the crafted item to the inventory
    /// </summary>
    public void AddToInventory()
    {
        if (item != null)
        {
            InventoryManager.Instance.AddItem(item, 1);
            item = null;
            animator.enabled = false;
            CraftManager.Instance.Reset();
        }
    }

    /// <summary>
    /// Easier way to add item to inventory on click with CMD/Ctrl held
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        // Detect CMD/Ctrl keys
        bool isCommandHeld = Input.GetKey(KeyCode.LeftCommand) ||
                            Input.GetKey(KeyCode.RightCommand) ||
                            Input.GetKey(KeyCode.LeftControl) ||
                            Input.GetKey(KeyCode.RightControl);

        if (isCommandHeld)
        {
            AddToInventory();
            InventoryManager.Instance.RefreshInventories();
        }
    }

    /// <summary>
    /// Show item description on pointer enter
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            CanvasGroup panel = GetComponentInParent<CanvasGroup>();
            InventoryManager.Instance.ShowItemDescription(panel, item);
        }
    }

    /// <summary>
    /// Hide item description on pointer exit
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            CanvasGroup panel = GetComponentInParent<CanvasGroup>();
            InventoryManager.Instance.HideItemDescription(panel);
        }
    }
}