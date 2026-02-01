using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CraftManager : MonoBehaviour
{
    [SerializeField] private DropZone leftCraftZone;
    [SerializeField] private DropZone rightCraftZone;
    [SerializeField] private List<Items> availableItems;
    [SerializeField] private Animator cauldronAnimator;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Image loadingBarInner;
    [SerializeField] private GameObject craftButton;
    [SerializeField] private Image resultItemImage;
    [SerializeField] private Image resultItemPlaceholder;
    [SerializeField] private Items randomPotion;
    [SerializeField] private Transform cookPoint;
    [SerializeField] private CraftResult craftResult;
    [SerializeField] private int timeToCraft = 2;
    private float craftTimer = 0f;
    private bool canCraft = false;
    private Items leftItem;
    private Items rightItem;
    private Items resultItem = null;
    private Coroutine craftCoroutine;
    private bool crafted = false;
    public static CraftManager Instance;
    public bool isCrafting = false;

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

    void Start()
    {
        cauldronAnimator = GameObject.Find("Cauldron").GetComponent<Animator>();
        if (resultItemPlaceholder != null) resultItemPlaceholder.enabled = true;
        if (resultItemImage != null) resultItemImage.enabled = false;
    }

    void Update()
    {
        leftItem = leftCraftZone != null ? leftCraftZone.GetItem() : null;
        rightItem = rightCraftZone != null ? rightCraftZone.GetItem() : null;
        canCraft = (leftItem != null || rightItem != null) && !isCrafting && !crafted;
        UpdateVisuals();
    }

    /// <summary>
    /// Start the crafting process on cook-button press
    /// </summary>
    public void Craft()
    {
        if (!canCraft || isCrafting) return;

        // Start coroutines to update loading bar & animate ingredients positions
        isCrafting = true;
        craftTimer = 0f;
        if (craftCoroutine != null) StopCoroutine(craftCoroutine);
        if (leftItem != null) StartCoroutine(CookIngredients(leftCraftZone));
        if (rightItem != null) StartCoroutine(CookIngredients(rightCraftZone));
        craftCoroutine = StartCoroutine(CraftingProcess());
    }

    /// <summary>
    /// Coroutine to animate ingredients moving to cauldron
    /// </summary>
    /// <param name="zone">Drop zone containing the ingredient</param>
    /// <returns></returns>
    private IEnumerator CookIngredients(DropZone zone)
    {
        GameObject clone = new();
        clone.transform.SetParent(cauldronAnimator.gameObject.transform);
        Transform iconPosition = zone.itemIcon.transform;
        clone.transform.position = iconPosition.position;

        Image cloneImage = clone.AddComponent<Image>();
        cloneImage.sprite = leftItem.itemSprite;
        cloneImage.preserveAspect = true;

        leftCraftZone.ClearZone();
        rightCraftZone.ClearZone();

        float cookTimer = 0f;
        Vector3 startPos = iconPosition.position;
        Vector3 endPos = cookPoint.position;

        // Lerp the item to the cauldron over .25 seconds
        while (cookTimer < .25f)
        {
            cookTimer += Time.deltaTime;
            clone.transform.position = Vector3.Lerp(startPos, endPos, cookTimer / .25f);
            cloneImage.rectTransform.sizeDelta = Vector3.Lerp(new Vector3(100, 100, 0), new Vector3(50, 50, 0), cookTimer / .25f);
            yield return null;
        }
        Destroy(clone);
        yield return null;
    }

    /// <summary>
    /// Update the crafting UI visuals
    /// </summary>
    public void UpdateVisuals()
    {
        if (craftButton != null) craftButton.SetActive(canCraft);
        resultItemImage.enabled = crafted && resultItem != null;
        resultItemPlaceholder.enabled = !isCrafting && !crafted;

        // Update cauldron animation based on crafting status
        if (cauldronAnimator) cauldronAnimator.SetBool("isCrafting", isCrafting);

        if (crafted && resultItem != null)
        {
            resultItemImage.sprite = resultItem.itemSprite;
            resultItemImage.preserveAspect = true;
            craftResult.SetItem(resultItem);
        }
    }

    /// <summary>
    /// Coroutine to handle the crafting process with loading bar
    /// </summary>
    /// <returns></returns>
    private IEnumerator CraftingProcess()
    {
        while (craftTimer < timeToCraft)
        {
            craftTimer += Time.deltaTime;
            loadingBarInner.fillAmount = craftTimer / timeToCraft;
            yield return null;
        }

        // Crafting complete
        isCrafting = false;
        loadingBarInner.fillAmount = 0f;
        crafted = true;
        yield return null;
    }

    /// <summary>
    /// Update the craft status and determine the result item based on
    /// provided ingredients
    /// </summary>
    public void UpdateCraftStatus()
    {
        if (IsCraftDisabled()) return;

        leftItem = leftCraftZone != null ? leftCraftZone.GetItem() : null;
        rightItem = rightCraftZone != null ? rightCraftZone.GetItem() : null;
        if (leftItem != null && rightItem != null)
        {
            resultItem = GetResultForDoubleCraft();
        }
        else if (leftItem != null || rightItem != null)
        {
            resultItem = GetResultForSingleCraft();
        }
        if (!resultItem)
        {
            resultItem = randomPotion;
        }
    }

    /// <summary>
    /// Check if crafting is currently disabled
    /// </summary>
    /// <returns></returns>
    public bool IsCraftDisabled()
    {
        return isCrafting || crafted;
    }

    /// <summary>
    /// Determine the result item for double ingredients crafting
    /// </summary>
    /// <returns></returns>
    private Items GetResultForDoubleCraft()
    {
        Craft leftCraft = new(leftItem, 1);
        Craft rightCraft = new(rightItem, 1);

        foreach (Items item in availableItems)
        {
            if (item.recipe.Count == 1 && leftItem == rightItem && item.recipe.Any(i => i.item == leftCraft.item && i.quantity == 2))
            {
                return item;
            }

            if (item.recipe.Count == 2)
            {
                // Check if boths ingredients match the recipe no matter the order
                if ((item.recipe[0].item == leftCraft.item  && item.recipe[1].item == rightCraft.item) ||
                    (item.recipe[0].item == rightCraft.item && item.recipe[1].item == leftCraft.item))
                {
                    return item;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Determine the result item for single ingredient crafting
    /// </summary>
    /// <returns>Item data (random potion if nothing matches)</returns>
    private Items GetResultForSingleCraft()
    {
        Craft providedCraft = new(leftItem != null ? leftItem : rightItem, 1);
        foreach (Items item in availableItems)
        {
            if (item.recipe.Count == 1 && item.recipe[0].item == providedCraft.item && item.recipe[0].quantity == 1)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Reset the crafting state
    /// </summary>
    public void Reset()
    {
        crafted = false;
        isCrafting = false;
        craftTimer = 0f;
        if (craftCoroutine != null) StopCoroutine(craftCoroutine);
        loadingBarInner.fillAmount = 0f;
        leftCraftZone.ClearZone();
        rightCraftZone.ClearZone();
        craftResult.SetItem(null);
        if (resultItemImage != null) resultItemImage.enabled = false;
        if (resultItemPlaceholder != null) resultItemPlaceholder.enabled = true;
        resultItem = null;
    }
}
