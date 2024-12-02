using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthMechanism : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Damage(int damage)
    {
        health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
