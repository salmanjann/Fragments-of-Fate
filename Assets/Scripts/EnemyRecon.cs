using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecon : MonoBehaviour
{
    public Transform point_1;
    public Transform point_2;
    public Transform enemy;
    public float enemySpeed;
    private Vector3 enemyLocalScale;
    private bool movingLeft;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        enemyLocalScale = enemy.localScale;
        animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        animator.SetBool("moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= point_1.position.x)
                enemyMovement(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= point_2.position.x)
                enemyMovement(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        animator.SetBool("moving", false);
        movingLeft = !movingLeft;
    }

    private void enemyMovement(int direction)
    {
        animator.SetBool("moving", true);

        enemy.localScale = new Vector3(Mathf.Abs(enemyLocalScale.x) * direction, enemyLocalScale.y, enemyLocalScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * enemySpeed,
            enemy.position.y, enemy.position.z);
    }
}
