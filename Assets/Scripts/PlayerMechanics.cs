using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    // this is the player sprite for manipulation on the object
    public GameObject sprite;
    // Speed that the player will be moving at
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        MovementManager();
    }

    // this function is responsible for managing anything involving character movement or controls
    private void MovementManager()
    {
        // horizontal movement
        float horizontal = Input.GetAxis("Horizontal");
        if(horizontal < 0 && !sprite.GetComponent<SpriteRenderer>().flipX)
        {
            // fix direction sprite is facing
            sprite.GetComponent<SpriteRenderer>().flipX = true;
            // // displace sprite to match positions of pngs
            // sprite.transform.position = new Vector2(sprite.transform.position.x - 1.33f, sprite.transform.position.y);
        }
        else if(horizontal > 0 && sprite.GetComponent<SpriteRenderer>().flipX)
        {
            // fix direction sprite is facing
            sprite.GetComponent<SpriteRenderer>().flipX = false;
            // // displace sprite to match positions of pngs
            // sprite.transform.position = new Vector2(sprite.transform.position.x + 1.33f, sprite.transform.position.y);
        }
        this.transform.position = new Vector2(this.transform.position.x + (horizontal * speed), this.transform.position.y);
    }
}
