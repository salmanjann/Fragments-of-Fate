using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown;
    public float range;
    public int damage;
    public float colliderDistance;
    public BoxCollider2D boxCollider;
    public LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    private Animator animator;
    private Health playerHealth;
    private EnemyRecon enemyRecon;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyRecon = GetComponentInParent<EnemyRecon>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("attack");
            }
        }

        if (enemyRecon != null)
            enemyRecon.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                                             new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                                             0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.takeDamage(damage);
    }
}
