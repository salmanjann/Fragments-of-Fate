using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    private Animator animator;
    private bool grounded = true;
    private bool powerUp = false;
    private Health enemyHealth;
    public Transform swordPoint;
    public float swordRange;
    public LayerMask enemyLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y); // Horizontal movement

        if (horizontalInput > 0)
            transform.localScale = Vector3.one;
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || powerUp))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (!powerUp) grounded = false; 
        }

        if (Input.GetKeyDown(KeyCode.F))
            Attack();

        animator.SetBool("Walk", horizontalInput != 0);
        animator.SetBool("isGrounded", grounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("Got powerup1");
            powerUp = true;
            StartCoroutine(ResetPowerUp());
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
        Collider2D[] hits = Physics2D.OverlapCircleAll(swordPoint.position, swordRange, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            enemyHealth = hit.GetComponent<Health>();
            if (enemyHealth != null)
            {
                Debug.Log("Enemy hit!");
                enemyHealth.takeDamage(1);
            }
        }
    }

    private IEnumerator ResetPowerUp()
    {
        yield return new WaitForSeconds(10f);
        powerUp = false;
    }
}
