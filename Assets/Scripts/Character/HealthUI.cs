using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Settings")]
    public GameObject heartPrefab; // A UI Image prefab
    public Character player;       // Reference to your Character script

    private List<Image> hearts = new List<Image>();
    private float maxHealth;

    void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<Character>();

        if (player != null)
        {
            maxHealth = player.health;
            InitializeHearts();
        }
    }

    void Update()
    {
        if (player != null)
        {
            UpdateHealthDisplay();
        }
    }

    // Creates the initial pool of heart icons based on max health
    void InitializeHearts()
    {
        // Clear existing
        foreach (Transform child in transform) Destroy(child.gameObject);
        hearts.Clear();

        // Create one heart icon for every 1.0 unit of max health
        for (int i = 0; i < Mathf.CeilToInt(maxHealth); i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            Image heartImage = newHeart.GetComponent<Image>();
            hearts.Add(heartImage);
        }
    }

    public void UpdateHealthDisplay()
    {
        float currentHealth = player.health;

        for (int i = 0; i < hearts.Count; i++)
        {
            // If health is 3, i=0 is full, i=1 is full, i=2 is full
            // If health is 2.5, i=0 full, i=1 full, i=2 half
            
            if (currentHealth >= i + 1f)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (currentHealth >= i + 0.5f)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}