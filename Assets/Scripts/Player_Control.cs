using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    private float speed;
    private bool speededUp;

    public float minX, maxX;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        speededUp = false;
        speed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void playerBound()
    {
        pos = this.transform.position;

        if (pos.x < minX)
        {
            pos.x = minX;
        }
        else if (pos.x > maxX)
        {
            pos.x = maxX;
        }

        this.transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerUp")
        {
            speededUp = true;
            Invoke("speedReset", 10f);
        }
    }
    private void FixedUpdate()
    {
        playerBound();

        if (speededUp)
        {
            HorizontalMovementSpeedup();
        }
    }
    private void HorizontalMovementSpeedup()
    {
        float horizontal = Input.GetAxis("Horizontal");
        this.transform.position = new Vector2(this.transform.position.x + (horizontal * speed), this.transform.position.y);
    }

    private void speedReset()
    {
        speededUp = false;
    }
}
