using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import for UI components


public class Mushroom_Movement : MonoBehaviour
{

    public float speed = 0.5f; // Speed of the mushroom
    private bool movingLeft = true; // Current movement direction
    private float minX; // Leftmost bound
    private float maxX; // Rightmost bound
    private Vector3 originalScale; // Store original scale for flipping
    private Animator animator; // Reference to the Animator

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Store the original scale of the mushroom
        originalScale = transform.localScale;

        // Get bounds from the parent Level1 script

        Level1 level = FindObjectOfType<Level1>();
        minX = level.spawnAreaMin.x;
        maxX = level.spawnAreaMax.x;

        if (level == null)
        {
            Debug.LogError("Level1 script not found!");
        }
        else
        {
            Debug.Log("Level1 script found!");
        }

        Debug.Log($"Bounds - MinX: {minX}, MaxX: {maxX}");

        // Set isWalking to true to start the Walking animation
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        // Move the mushroom left or right based on the direction
        if (movingLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Check if the mushroom has reached the leftmost bound
            if (transform.position.x <= minX)
            {
                movingLeft = false; // Switch direction to right
                Flip(); // Flip the mushroom sprite
            }
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Check if the mushroom has reached the rightmost bound
            if (transform.position.x >= maxX)
            {
                movingLeft = true; // Switch direction to left
                Flip(); // Flip the mushroom sprite
            }
        }
    }
        // Flip the mushroom sprite to face the correct direction
        void Flip()
    {
        transform.localScale = new Vector3(
            movingLeft ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
    }

}