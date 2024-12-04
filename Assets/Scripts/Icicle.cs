using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    // see if the player is in the range of the falling icicle
    public BoxCollider2D vision;
    // check if the force has been applied or not
    private bool launched;
    // Start is called before the first frame update
    void Start()
    {
        launched = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        RemoveIcicle();
    }
    // makes sure icicle is destroyed when out of bounds
    private void RemoveIcicle()
    {
        if(this.transform.position.y < -10f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !launched)
        {
            launched = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnDrawGizmos()
    {

        if(vision != null)
        {
            // DRAW THE VISION COLLIDER
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(vision.bounds.center, vision.bounds.size);
        }
    }
}
