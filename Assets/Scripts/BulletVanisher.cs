using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVanisher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "bullet"){
            Destroy(other.gameObject);
        }
    }
}
