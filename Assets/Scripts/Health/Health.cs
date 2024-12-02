using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float startingHeatlh;
    public float currentHealth;
    private Animator animator;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHeatlh;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       /*if(Input.GetKeyDown(KeyCode.E)) 
       {
            takeDamage(1);
       }*/
    }

    public void takeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHeatlh);
        Debug.Log(currentHealth);
        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                animator.SetTrigger("die");
                if(GetComponent<PlayerMovement>() != null) 
                    GetComponent<PlayerMovement>().enabled = false;
                if (GetComponentInParent<EnemyRecon>() != null)
                    GetComponent<EnemyRecon>().enabled = false;
                if (GetComponent<EnemyAttack>() != null)
                    GetComponent<EnemyAttack>().enabled = false;
                dead = true;
                if (this.CompareTag("Enemy"))
                {
                    BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
                    if (boxCollider != null)
                    {
                        Destroy(boxCollider);
                    }
                }
            }
        }
    }
}
