using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import for UI components

public class Level1 : MonoBehaviour

{

    public float move;
    public float speed;
    public float jumpForce;
    public bool isjumping;
    public Rigidbody2D rb;
    public GameObject coinPrefab; // Coin prefab
    public GameObject spawnArea; // Background GameObject for spawn area
    public Vector2 spawnAreaMin; // Bottom-left corner
    public Vector2 spawnAreaMax; // Top-right corner
    public int numberOfCoins = 10; // Number of coins to spawn
    public float coinOffset = 0.5f;
    private float minX; // Leftmost limit
    private float maxX; // Rightmost Limit
    private float maxY; // Maximum Y position for the player
    private float navbarBottomY; // Bottom of the Navbar
    private Vector3 originalScale; // Original size of player
    public Image heartFill; // Reference to the heart fill image
    public float maxHealth = 100f; // Max health
    private float currentHealth; // Current health
    public GameObject potionPrefab; // Potion prefab
    public GameObject keyPrefab; // Key prefab
    public float minPotionDistance = 5f; // Minimum distance from the player for potion
    public float minKeyDistance = 5f; // Minimum distance from player and potion for keys
    public Text scoreText; // Reference to the Text object
    private int score = 0; // Variable to store the player's score
    private int keysCollected = 0; // Count of keys collected
    public Animator doorAnimator; // Public Animator for the door


    void Start()
    {
        originalScale = transform.localScale;

        Bounds bounds = spawnArea.GetComponent<Renderer>().bounds;

        // Set spawn area min and max
        spawnAreaMin = new Vector2(bounds.min.x + coinOffset, bounds.min.y + coinOffset);
        spawnAreaMax = new Vector2(bounds.max.x - coinOffset, bounds.max.y - coinOffset);

        Debug.Log($"Level1 - spawnAreaMin: {spawnAreaMin}, spawnAreaMax: {spawnAreaMax}");

        minX = bounds.min.x + 0.8f;
        maxX = bounds.max.x - 0.8f;

        // Find the Navbar and adjust the top limit for coin spawning
        GameObject navbar = GameObject.FindGameObjectWithTag("Navbar");
        if (navbar != null)
        {
            Bounds navbarBounds = navbar.GetComponent<Renderer>().bounds;
            navbarBottomY = navbarBounds.min.y - coinOffset; // Just below the Navbar
            spawnAreaMax = new Vector2(spawnAreaMax.x, navbarBottomY); // Adjust top Y limit
            maxY = navbarBottomY;
        }

        // Initialize health
        currentHealth = maxHealth;
        UpdateHealthBar();

        potionPrefab.SetActive(false);

        keyPrefab.SetActive(false); // Hide the initial key

        // Spawn coins
        SpawnCoins();
        SpawnPotion();
        SpawnKeys();

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnCollisionEnter2D(Collision2D collision)

    {

        if (collision.gameObject.CompareTag("Mushroom"))

        {
            // Get the contact point and the mushroom's bounds
            Vector2 contactPoint = collision.GetContact(0).point;
            Bounds mushroomBounds = collision.gameObject.GetComponent<Collider2D>().bounds;

            // Check if the contact point is above the mushroom's center
            if (contactPoint.y > mushroomBounds.max.y - 0.1f) // Adjust offset for precision
            {
                // Destroy the mushroom if the player's feet land on it
                Destroy(collision.gameObject);
            }
            else
            {
                // Take damage if the collision is not from above
                TakeDamage(20f);
            }
        }

    }

    void SpawnCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            // Generate random X and Y positions within bounds
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            // Create a new spawn position
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // Instantiate the coin at the random position
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnPotion()
    {
        Vector2 potionPosition;
        do
        {
            // Generate random X and Y positions within bounds
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            // Set the position for the potion
            potionPosition = new Vector2(randomX, randomY);
        }
        while (Vector2.Distance(potionPosition, transform.position) < minPotionDistance);

        // Instantiate the potion at the random position
        GameObject newPotion = Instantiate(potionPrefab, potionPosition, Quaternion.identity);
        newPotion.SetActive(true); // Ensure the new potion is visible
    }

    void SpawnKeys()
    {
        for (int i = 0; i < 3; i++) // Spawn 3 keys
        {
            Vector2 keyPosition;
            do
            {
                // Generate random X and Y positions within bounds
                float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

                // Set the position for the key
                keyPosition = new Vector2(randomX, randomY);
            }
            while (Vector2.Distance(keyPosition, transform.position) < minKeyDistance || // Far from player
                   Vector2.Distance(keyPosition, potionPrefab.transform.position) < minKeyDistance); // Far from potion

            // Instantiate the key at the random position
            GameObject newKey = Instantiate(keyPrefab, keyPosition, Quaternion.identity);
            newKey.SetActive(true); // Ensure the new key is visible
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with a coin
        if (collision.gameObject.CompareTag("Coin"))
        {
            // Destroy the coin

            // Increase score
            score += 10;

            // Update the score text
            UpdateScoreText();

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("potion"))
        {
            // Increase player's health by 25% of max health
            float healthRestoration = maxHealth * 0.25f;
            currentHealth += healthRestoration;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed max health

            // Update the health bar
            UpdateHealthBar();

            // Destroy the potion object
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("key"))

        {
            // Collect key
            keysCollected++;
            Debug.Log(keysCollected);
            Destroy(collision.gameObject);

            // Check if all keys are collected
            if (keysCollected == 3)
            {
                UnlockDoor();
            }
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            // Destroy the coin
            Destroy(collision.gameObject);
        }

        else if (!collision.gameObject.CompareTag("door"))

        {

            // do nothing

        }
    }
    // Reduce the health
    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the player if health is 0
        }
    }

    // Update the health bar fill
    private void UpdateHealthBar()
    {
        if (heartFill != null)
        {
            heartFill.fillAmount = currentHealth / maxHealth;
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }


    private void UnlockDoor()

    {
     
            // Set the Animator parameter to open the door
        doorAnimator.SetBool("isOpen", true);
        Debug.Log("Hi");

    }


}