using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStageMechanics : MonoBehaviour
{
    // this is the force that is gonna be acting on the object to make hims slide
    public float slideforce;
    // these are players feet to make sure force is only added when the player is on ground and not other wise
    private BoxCollider2D feet;
    // make record if player is on ground at any given moment of game
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        feet = this.GetComponent<PlayerMechanics>().feet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Sliding();
    }
    // this function is responsible for making player slide as he moves on snowy field
    private void Sliding()
    {
        if(!grounded)
            return;
        float direction_factor = Input.GetAxis("Horizontal");
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(slideforce*direction_factor,0f));
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object the feet touched is tagged "Ground"
        if (feet.IsTouching(collision) && collision.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the object the feet was touching is tagged "Ground"
        if (!feet.IsTouching(collision) && collision.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

}
