using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireShieldLevel3 : MonoBehaviour
{
    // shield sprite
    public GameObject sprite;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("removeShield",15f);
        Invoke("Blinker",10f);
    }
    // destroy the shield when the timer runs out
    private void removeShield()
    {
        Destroy(this.gameObject);
    }
    // makes the shield blink at specefic time intervals
    private void Blinker()
    {
        sprite.GetComponent<SpriteRenderer>().enabled = !sprite.GetComponent<SpriteRenderer>().enabled;
        Invoke("Blinker",20/60f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.CompareTag("Enemy"))
        {
            EnemyHealthMechanism healthmechanism = obj.GetComponent<EnemyHealthMechanism>();
            if(healthmechanism != null)
            {
                float force = obj.transform.position.x - this.transform.position.x;
                if(force > 0)
                {
                    force = 300f;
                }
                else
                {
                    force = -300f;
                }
                obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(force,Math.Abs(force)));
                healthmechanism.Damage(1);
            }
        }
    }
}
