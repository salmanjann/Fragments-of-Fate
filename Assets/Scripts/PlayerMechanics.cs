using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMechanics : MonoBehaviour
{
    public int maxJump;
    // Collider on feet
    public Collider2D feet;
    // this is the player sprite for manipulation on the object
    public GameObject sprite;
    // Speed that the player will be moving at
    private float speed;
    // current number of jumps
    private int jumps;
    // Start is called before the first frame update

    public float minX, maxX;
    private Vector3 pos;
    void Start()
    {
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void playerBound()
    {
        pos = this.transform.position;

        if (pos.x < minX)
        {
            pos.x = minX;
        }
        else if (pos.x > maxX)
        {
            pos.x = maxX;
        }

        this.transform.position = pos;
    }

    private void FixedUpdate()
    {
        MovementManager();
        playerBound();
    }

    // this function is responsible for managing anything involving character movement or controls
    private void MovementManager()
    {
        HorizontalMovement();
        Jumps();
    }
    // this function is responsible for the jump mechanic 
    private void Jumps()
    {
        if(Input.GetButtonDown("Jump") && jumps > 0)
        {
            float JumpForce = 325f;
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0,JumpForce));
            jumps--;
        }
    }
    // this function is responsible for anything that happens for and during horizontal movement of player
    private void HorizontalMovement()
    {
        // horizontal movement variable
        float horizontal = Input.GetAxis("Horizontal");
        // set move variable for the animator to make him run or become idle
        sprite.GetComponent<Animator>().SetFloat("move",Math.Abs(horizontal));
        // Sprite flipping if needed
        if(horizontal < 0 && !sprite.GetComponent<SpriteRenderer>().flipX)
        {
            // fix direction sprite is facing
            sprite.GetComponent<SpriteRenderer>().flipX = true;
            // fix Colliders placement
            Collider2D[] colliders = this.GetComponents<Collider2D>();
            for(int i = 0; i < colliders.Length; i++)
            {
                colliders[i].offset = new Vector2(colliders[i].offset.x + 1.315949f, colliders[i].offset.y);
            }
            // // displace sprite to match positions of pngs
            // sprite.transform.position = new Vector2(sprite.transform.position.x - 1.33f, sprite.transform.position.y);
        }
        else if(horizontal > 0 && sprite.GetComponent<SpriteRenderer>().flipX)
        {
            // fix direction sprite is facing
            sprite.GetComponent<SpriteRenderer>().flipX = false;
            // fix Colliders placement
            Collider2D[] colliders = this.GetComponents<Collider2D>();
            for(int i = 0; i < colliders.Length; i++)
            {
                colliders[i].offset = new Vector2(colliders[i].offset.x - 1.315949f, colliders[i].offset.y);
            }
            // // displace sprite to match positions of pngs
            // sprite.transform.position = new Vector2(sprite.transform.position.x + 1.33f, sprite.transform.position.y);
        }
        this.transform.position = new Vector2(this.transform.position.x + (horizontal * speed), this.transform.position.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object's feet touched is tagged "Ground"
        if (feet.IsTouching(collision) && collision.CompareTag("Ground"))
        {
            jumps = maxJump;
        }
        if (collision.tag == "PowerUp")
        {
            speed = 4f;
        }
    }

    private void OnDrawGizmos()
    {
        // DRAW THE FEET COLLIDER
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(feet.bounds.center, feet.bounds.size);
    }
}
