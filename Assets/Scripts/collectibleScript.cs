using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectibleScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {

            this.gameObject.SetActive(false);
        }
    }
}
