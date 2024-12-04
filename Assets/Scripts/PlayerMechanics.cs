using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMechanics : MonoBehaviour
{
    // original colour of the sprite
    private Color originalColor;
    // force that is applied to rigid body
    public float JumpForce;
    // this is the slider for hp in ui of game
    public Slider HpBar;
    // as name suggest it is max health of player
    public int MaxHEALTH;
    // this is the damage that the player will be giving to the opponents
    public int damage;
    // number of jumps the player can perform
    public int MAXJUMPS;
    public BoxCollider2D mainCollider;

    // Collider on feet
    public BoxCollider2D feet;
    public BoxCollider2D feet2;
    // Collider on body
    public BoxCollider2D body;
    // this is the player sprite for manipulation on the object
    public GameObject sprite;
    // this is the current health of player
    private int health;
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
    private BoxCollider2D attack_box;
    public SpriteRenderer spriteRenderer;

    // level4, comment this out
    public Level4_UI level4_UIRef;
    void Start()
    {
        originalColor = sprite.GetComponent<SpriteRenderer>().color;
        health = MaxHEALTH;
        HealthBarManager();
        attack_box = null;
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
        HealthBarManager();
        MovementManager();
    }
    private void HealthBarManager()
    {
        HpBar.value = (float)health / (float)MaxHEALTH;
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
        if (!attacking && grounded && Input.GetKeyDown(KeyCode.Z))
        {
            attacking = true;
            sprite.GetComponent<Animator>().SetTrigger("attack");
            // setup attack hitbox
            attack_box = this.gameObject.AddComponent<BoxCollider2D>();
            attack_box.offset = new Vector2(0.731f, -0.797f);
            if (sprite.GetComponent<SpriteRenderer>().flipX)
                attack_box.offset = new Vector2(-0.730f, -0.797f);
            attack_box.size = new Vector2(4.222319f, 4.27028f);
            attack_box.isTrigger = true;
            // make sure new attack can be performed on animation exit
            Invoke("ResetAttack", 20f / 60f);
            Invoke("destroyAttackBox", 16f / 60f);
        }
    }
    private void destroyAttackBox()
    {
        var temp = attack_box;
        attack_box = null;
        Destroy(temp);
    }
    // this finction resets attack after certain time as dev invokes to make sure player cannot add another attack in mean time of the current animation
    private void ResetAttack()
    {
        attacking = false;
    }
    // this function is responsible for the jump mechanic 
    private void Jumps()
    {
        if (Input.GetButtonDown("Jump") && jumps > 0)
        {
            grounded = false;
            jumpbuffer = true;
            // jump animation plays
            sprite.GetComponent<Animator>().SetTrigger("jump");
            // getting rigidbody and applying force
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, JumpForce));
            jumps--;
        }
    }
    // this function is responsible for anything that happens for and during horizontal movement of player
    private void HorizontalMovement()
    {
        // horizontal movement variable
        float horizontal = Input.GetAxis("Horizontal");
        // set move variable for the animator to make him run or become idle
        sprite.GetComponent<Animator>().SetFloat("move", Math.Abs(horizontal));
        // Sprite flipping if needed
        if (horizontal < 0 && !sprite.GetComponent<SpriteRenderer>().flipX)
        {
            // fix direction sprite is facing
            sprite.GetComponent<SpriteRenderer>().flipX = true;
            // fix Colliders placement
            Collider2D[] colliders = this.GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].offset = new Vector2(colliders[i].offset.x + 1.315949f, colliders[i].offset.y);
            }
        }
        else if (horizontal > 0 && sprite.GetComponent<SpriteRenderer>().flipX)
        {
            // fix direction sprite is facing
            sprite.GetComponent<SpriteRenderer>().flipX = false;
            // fix Colliders placement
            Collider2D[] colliders = this.GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].offset = new Vector2(colliders[i].offset.x - 1.315949f, colliders[i].offset.y);
            }
        }
        this.transform.position = new Vector2(this.transform.position.x + (horizontal * speed), this.transform.position.y);
    }
    // reset to default sprite colours
    private void ResetColor()
    {
        sprite.GetComponent<SpriteRenderer>().color = originalColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the Player is hit by an enemy
        if (body.IsTouching(collision) && collision.CompareTag("Enemy"))
        {
            EnemyHealthMechanism enemyScript = collision.GetComponent<EnemyHealthMechanism>();
            health -= enemyScript.damage;
            float force = this.transform.position.x - collision.transform.position.x;
            if (force > 0)
            {
                force = 100f;
            }
            else
            {
                force = -100f;
            }
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0f));
            sprite.GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("ResetColor", 1f);
        }
        // Check if the Player is hit by an bullet
        if (body.IsTouching(collision) && collision.CompareTag("bullet"))
        {
            health -= 10;
            float force = this.transform.position.x - collision.transform.position.x;
            if (force > 0)
            {
                force = 80f;
            }
            else
            {
                force = -80f;
            }
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0f));
            sprite.GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("ResetColor", 1f);
            Destroy(collision.gameObject);
        }
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
        // Check if the object is attacking "Enemy"
        if (attack_box != null && attack_box.IsTouching(collision) && collision.CompareTag("Enemy"))
        {
            EnemyHealthMechanism healthmechanism = collision.GetComponent<EnemyHealthMechanism>();
            if (healthmechanism != null)
            {
                float force = collision.transform.position.x - this.transform.position.x;
                if (force > 0)
                {
                    force = 300f;
                }
                else
                {
                    force = -300f;
                }
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0f));
                healthmechanism.Damage(damage);
            }
        }

        if (collision.gameObject.CompareTag("Exit"))
        {
            if (level4_UIRef.shardsCount != 5)
            {
                level4_UIRef.ShowShardsError();
            }
            else
            {
                level4_UIRef.ShowLevelComplete();
            }
        }

    }
    private void VisibleAgain()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        feet.enabled = true;
        body.enabled = true;
        feet2.enabled = false;
        mainCollider.enabled = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the object the feet was touching is tagged "Ground"
        if (!feet.IsTouching(collision) && collision.CompareTag("Ground"))
        {
            jumpbuffer = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (feet2.enabled == true)
        {
            if (!feet2.IsTouching(collision.collider) && collision.gameObject.CompareTag("Ground"))
            {
                jumpbuffer = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (feet != null)
        {
            // DRAW THE FEET COLLIDER
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(feet.bounds.center, feet.bounds.size);
        }
        if (attack_box != null)
        {
            // DRAW THE ATTACK COLLIDER
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attack_box.bounds.center, attack_box.bounds.size);
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawCube(attack_box.bounds.center, attack_box.bounds.size);
        }
    }

    private void DeactivateColliders()
    {
        feet.enabled = false;
        body.enabled = false;
        mainCollider.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Invisible"))
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);

            feet2.enabled = true;
            Invoke("DeactivateColliders", 0.5f);

            Destroy(other.gameObject);
            Invoke("VisibleAgain", 5f);
        }
        // Check if the object the feet touched is tagged "Ground"
        if (feet2.IsTouching(other.collider) && other.gameObject.CompareTag("Ground"))
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
        if (other.gameObject.CompareTag("Collectible"))
        {
            level4_UIRef.UpdateCount();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("DeadZone"))
        {
            sprite.GetComponent<Animator>().SetTrigger("Dead");
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            Destroy(other.gameObject);
            Invoke("GameOver", 1f);
        }
    }
    public void GameOver()
    {
        sprite.GetComponent<Animator>().SetTrigger("GameOver");
    }
}
