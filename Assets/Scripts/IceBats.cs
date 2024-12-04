using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class IceBats : MonoBehaviour
{
    // see if invoke for the zero velocity is called
    private bool velocityReset;
    // see if the bat is chasing player
    private bool chasing;
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
        velocityReset = false;
        chasing = false;
        speed = 0.05f;
        player = null;
        bat_animator = this.GetComponent<Animator>();
        startPosition = this.transform.position;
        // make sure speed increments are always there and bat is approaching
        if(speedIncrements <= 1)
        {
            speedIncrements = 1.1f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        ReduceForcesToZero();
        Movement();
    }
    // Bat Movement Logic
    private void Movement()
    {
        if(chasing)
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
            bat_animator.enabled = true;
        }
        else
        {
            Vector2 move = startPosition - new Vector2(this.transform.position.x, this.transform.position.y);
            move.Normalize();
            move *= 0.075f;
            this.transform.position = new Vector2(this.transform.position.x + move.x, this.transform.position.y + move.y);
        }
    }
    // Reduce the Forces acting on the bat
    private void ReduceForcesToZero()
    {
        if(velocityReset)
        {
            return;
        }
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if(rb.velocity.magnitude > 0.01f)
        {
            velocityReset = false;
            Invoke("VelocityReset",2f);
        }
    }
    // reset forces and velocities to zero
    private void VelocityReset()
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        velocityReset = false;
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
            chasing = true;
            player = obj.gameObject;
            bat_animator.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D obj)
    {
        if(vision.IsTouching(obj) && obj.CompareTag("Player"))
        {
            chasing = false;
            player = null;
            speed = 0.05f;
        }
    }
    private void OnDrawGizmos()
    {
        if(vision != null)
        {
            // DRAW THE VISION COLLIDER
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(vision.bounds.center, vision.radius);
        }
    }
}
