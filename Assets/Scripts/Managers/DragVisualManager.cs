using UnityEngine;
using UnityEngine.UI;

public class DragVisualManager : MonoBehaviour
{
    public static DragVisualManager Instance;

    public Canvas canvas;
    public GameObject dragImagePrefab;

    private GameObject currentDragObject;
    private Image dragImage;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>();
        }
    }

    /// <summary>
    /// Starts the drag visual with the given item sprite.
    /// </summary>
    /// <param name="itemSprite">Item sprite to clone</param>
    public void StartDrag(Sprite itemSprite)
    {
        if (currentDragObject != null)
            Destroy(currentDragObject);

        currentDragObject = Instantiate(dragImagePrefab, canvas.transform);
        dragImage = currentDragObject.GetComponent<Image>();
        dragImage.sprite = itemSprite;
        dragImage.raycastTarget = false;
        Color color = dragImage.color;
        color.a = 0.7f;
        dragImage.color = color;
    }

    /// <summary>
    /// Updates the drag visual position
    /// </summary>
    /// <param name="position">Vector 2 current position</param>
    public void UpdateDragPosition(Vector2 position)
    {
        if (currentDragObject != null)
        {
            currentDragObject.transform.position = position;
        }
    }

    /// <summary>
    /// Ends the drag visual and destroy clone
    /// </summary>
    public void EndDrag()
    {
        if (currentDragObject != null)
        {
            Destroy(currentDragObject);
            currentDragObject = null;
            dragImage = null;
        }
    }
}