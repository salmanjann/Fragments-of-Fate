using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthMechanism : MonoBehaviour
{
    public int health;
    public GameObject sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Damage(int damage)
    {
        health -= damage;
        sprite.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor",1f);
    }
    private void ResetColor()
    {
        sprite.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
