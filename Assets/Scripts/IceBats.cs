using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class IceBats : MonoBehaviour
{
    // this is the Maxspeed of the bat as it chases the player
    public float MaxSpeed;
    // this is the increment factor for the speed of the bat
    public float speedIncrements;
    // this is the current speed of the bat
    private float speed;
    // vision of the bat
    public CircleCollider2D vision;
    // initial position of the bat
    private Vector2 startPosition;
    // this is the player if the player is detected in scene
    private GameObject player;
    // this is the Animator on the bat to make sure its animations
    private Animator bat_animator;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.05f;
        player = null;
        bat_animator = this.GetComponent<Animator>();
        startPosition = this.transform.position;
        // make sure speed increments are always there and bat is approaching
        if(speedIncrements < 2)
        {
            speedIncrements = 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(player != null)
        {
            FollowPlayer();
        }
        else
        {
            ReturnToStart();
        }
    }
    // this function is supposed to send the bat to the starting point
    private void ReturnToStart()
    {
        if(Vector2.Distance(startPosition, this.transform.position.ConvertTo<Vector2>()) <= 0.075f)
        {
            this.transform.position = startPosition;
            bat_animator.SetTrigger("arc");
        }
        else
        {
            Vector2 move = startPosition - this.transform.position.ConvertTo<Vector2>();
            move.Normalize();
            move *= 0.075f;
            this.transform.position = new Vector2(this.transform.position.x + move.x, this.transform.position.y + move.y);
        }
    }
    // this function is to attack the player by following him at a certain speed
    private void FollowPlayer()
    {
        Vector2 move = player.transform.position - this.transform.position;
        move.Normalize();
        move *= speed;
        this.transform.position = new Vector2(this.transform.position.x + move.x, this.transform.position.y + move.y);
        speed *= speedIncrements;
        if(speed > MaxSpeed)
        {
            speed = MaxSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(vision.IsTouching(obj) && obj.CompareTag("Player"))
        {
            bat_animator.SetTrigger("still");
            player = obj.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D obj)
    {
        if(vision.IsTouching(obj) && obj.CompareTag("Player"))
        {
            player = null;
            speed = 0.05f;
        }
    }

    private void OnDrawGizmos()
    {
        if(vision != null)
        {
            // DRAW THE FEET COLLIDER
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(vision.bounds.center, vision.radius);
        }
    }
}
