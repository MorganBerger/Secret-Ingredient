using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private GameObject messageBox;
    [SerializeField] private Items item;

    [SerializeField] private bool showMessage = false;

    private void Awake()
    {
        messageBox.SetActive(false);
    }
    private void Start()
    {
        messageBox.SetActive(false);
    }

    private void Update()
    {
        if (showMessage)
        {
            messageBox.SetActive(true);
        }
        else
        {
            messageBox.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && showMessage)
        {
            InventoryManager.Instance.AddItem(item, 1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            showMessage = true;
        }
    }

  private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            showMessage = false;
        }
    }
}
