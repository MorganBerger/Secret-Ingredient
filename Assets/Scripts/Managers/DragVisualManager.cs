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

    public void UpdateDragPosition(Vector2 position)
    {
        if (currentDragObject != null)
        {
            currentDragObject.transform.position = position;
        }
    }

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