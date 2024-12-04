using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsing_Platform : MonoBehaviour
{
    private float fallDelay;
    private float destryDelay;
    private Vector2 startPosition;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        fallDelay = 1f;
        destryDelay = 2f;
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(destryDelay);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        this.transform.position = startPosition;
    }
}
