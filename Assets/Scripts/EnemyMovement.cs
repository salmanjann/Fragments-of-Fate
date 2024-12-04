using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float absDistance;
    public float speed;
    public bool isMoveLeft = true;
    private Rigidbody2D rb;
    private float leftBoundary;
    private float rightBoundary;
    private float direction = 1.0f;
    public LayerMask detectionLayer;
    private Animator animator;
    private bool isAttacking = false;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private bool isShooting = false; // To prevent multiple InvokeRepeats

    void Start()
    {
        // Calculate movement boundaries
        leftBoundary = transform.position.x - absDistance / 2;
        rightBoundary = transform.position.x + absDistance / 2;
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        if (isMoveLeft)
        {
            MoveLeft();
            FlipSprite();
        }
        else
            MoveRight();
    }

    private void FixedUpdate()
    {
        if (isMoveLeft && !isAttacking)
        {
            rb.velocity = new Vector2(speed * direction, 0.0f);
            if (transform.position.x <= leftBoundary)
            {
                MoveRight();
                FlipSprite();
            }
        }
        else if (!isMoveLeft && !isAttacking)
        {
            rb.velocity = new Vector2(speed * direction, 0.0f);
            if (transform.position.x >= rightBoundary)
            {
                MoveLeft();
                FlipSprite();
            }
        }
        RaycastOnBothSides();
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invert the x-axis
        transform.localScale = scale;
    }

    private void RaycastOnBothSides()
    {
        float rightRayLength = Mathf.Abs(rightBoundary - transform.position.x);
        float leftRayLength = Mathf.Abs(leftBoundary - transform.position.x);
        Vector2 origin = transform.position;

        RaycastHit2D rightHit = Physics2D.Raycast(origin, Vector2.right, rightRayLength, detectionLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(origin, Vector2.left, leftRayLength, detectionLayer);

        Debug.DrawRay(origin, Vector2.right * rightRayLength, Color.white);
        Debug.DrawRay(origin, Vector2.left * leftRayLength, Color.green);

        if (rightHit.collider != null && rightHit.collider.gameObject != gameObject)
        {
            if (rightHit.collider.name == "Player" && !isAttacking)
            {
                Attack();
                if (isMoveLeft)
                {
                    FlipSprite();
                    MoveRight();
                }
                if (!isShooting)
                {
                    isShooting = true;
                    InvokeRepeating(nameof(ShootRight), 1f, 2f);
                }
            }
        }

        if (leftHit.collider != null && leftHit.collider.gameObject != gameObject)
        {
            if (leftHit.collider.name == "Player" && !isAttacking)
            {
                Attack();
                if (!isMoveLeft)
                {
                    FlipSprite();
                    MoveLeft();
                }
                if (!isShooting)
                {
                    isShooting = true;
                    InvokeRepeating(nameof(ShootLeft), 1f, 2f);
                }
            }
        }

        if (rightHit.collider == null && leftHit.collider == null && isAttacking )
        {
            NotAttack();
        }

        if (rightHit.collider == null && leftHit.collider == null && isShooting)
        {
            isShooting = false;
            CancelInvoke(nameof(ShootRight));
            CancelInvoke(nameof(ShootLeft));
        }
    }

    private void MoveLeft()
    {
        isMoveLeft = true;
        direction = -1.0f;
    }

    private void MoveRight()
    {
        isMoveLeft = false;
        direction = 1.0f;
    }

    private void Attack()
    {
        rb.velocity = new Vector2(0.0f, 0.0f);
        isAttacking = true;
        animator.SetTrigger("attack");
        Invoke("Idle", 1f);
        animator.ResetTrigger("walk");
    }

    private void Idle()
    {
        animator.SetTrigger("idle");
        animator.ResetTrigger("attackBack");

        Invoke("AttackBack", 1f);
    }
    private void AttackBack()
    {
        animator.SetTrigger("attackBack");
        animator.ResetTrigger("idle");
        if (isAttacking)
            Invoke("Idle", 1f);
    }

    private void NotAttack()
    {
        isAttacking = false;
        animator.SetTrigger("walk");
        animator.ResetTrigger("attack");
        animator.ResetTrigger("attackBack");
        animator.ResetTrigger("idle");
    }

    private void ShootRight()
    {
        ShootBullet(Vector2.right);
    }

    private void ShootLeft()
    {
        ShootBullet(Vector2.left);
    }

    private void ShootBullet(Vector2 direction)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab is not assigned!");
            return;
        }

        Vector2 bulletInstancePosition = direction == Vector2.left ? new Vector2(transform.position.x - 0.5f, transform.position.y -0.8f) : new Vector2(transform.position.x + 0.5f, transform.position.y -0.8f);
        GameObject bullet = Instantiate(bulletPrefab, bulletInstancePosition, Quaternion.identity);
        if (direction == Vector2.left)
        {
            Vector3 scale = bullet.transform.localScale;
            scale.x *= -1; // Invert the x-axis
            bullet.transform.localScale = scale;
        }
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.velocity = direction * bulletSpeed;
        }
    }
}
