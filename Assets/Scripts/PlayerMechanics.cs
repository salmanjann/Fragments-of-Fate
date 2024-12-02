using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    // number of jumps the player can perform
    public int MAXJUMPS;
    // Collider on feet
    public Collider2D feet;
    // this is the player sprite for manipulation on the object
    public GameObject sprite;
    // Speed that the player will be moving at
    private float speed;
    // current number of jumps
    private int jumps;
    // makes sure jumps dont get unregistered imidiately
    private bool jumpbuffer;
    // make record if player is on ground at any given moment of game
    private bool grounded;
    // make record if player is attacking kinda like buffer for next attack to work
    private bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        grounded = false;
        speed = 0.1f;
        jumpbuffer = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void FixedUpdate()
    {
        MovementManager();
    }

    // this function is responsible for managing anything involving character movement or controls
    private void MovementManager()
    {
        HorizontalMovement();
        Jumps();
        Attack();
    }
    // this function is responsible for attack function of the player
    private void Attack()
    {
        if(!attacking && grounded && Input.GetKeyDown(KeyCode.Z))
        {
            attacking = true;
            sprite.GetComponent<Animator>().SetTrigger("attack");
            Invoke("ResetAttack",20f/60f);
        }
    }
    // this finction resets attack after certain time as dev invokes to make sure player cannot add another attack in mean time of the current animation
    private void ResetAttack()
    {
        attacking = false;
    }
    // this function is responsible for the jump mechanic 
    private void Jumps()
    {
        if(Input.GetButtonDown("Jump") && jumps > 0)
        {
            grounded = false;
            jumpbuffer = true;
            // jump animation plays
            sprite.GetComponent<Animator>().SetTrigger("jump");
            // force that is applied to rigid body
            float JumpForce = 300f;
            // getting rigidbody and applying force
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
        // Check if the object the feet touched is tagged "Ground"
        if (feet.IsTouching(collision) && collision.CompareTag("Ground"))
        {
            if (!jumpbuffer)
            {
                jumps = MAXJUMPS;
                // change animation
                sprite.GetComponent<Animator>().SetTrigger("grounded");
                // since ground was touched the player is grounded
                grounded = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the object the feet was touching is tagged "Ground"
        if (!feet.IsTouching(collision) && collision.CompareTag("Ground"))
        {
            jumpbuffer = false;
        }
    }
    private void OnDrawGizmos()
    {
        // DRAW THE FEET COLLIDER
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(feet.bounds.center, feet.bounds.size);
    }
}
