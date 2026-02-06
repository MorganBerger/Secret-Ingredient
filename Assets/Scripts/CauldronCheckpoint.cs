using System.Collections;
using UnityEngine;

public class CauldronCheckpoint : MonoBehaviour
{
    [SerializeField] private GameObject messageBox;

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
            UIManager.Instance.ShowCraftMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            showMessage = true;
            GameManager.Instance.SetCheckpoint(gameObject.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            showMessage = false;
            UIManager.Instance.HideCraftMenu();
        }
    }
}
