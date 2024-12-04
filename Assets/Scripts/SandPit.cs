using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandPit : MonoBehaviour
{
    public float slowDownFactor = -0.5f; 
    public float sinkSpeed = 2f; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Control player = other.GetComponent<Player_Control>();
            if (player != null)
            {
                player.ModifySpeed(slowDownFactor);
                StartCoroutine(SinkPlayer(other.transform));
            }
        }
    }

    private IEnumerator SinkPlayer(Transform player)
    {
        Vector3 startPosition = player.position;
        Vector3 targetPosition = startPosition + new Vector3(0, -1f, 0); 
        float elapsedTime = 0;

        while (elapsedTime < 1.25f)
        {
            player.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / sinkSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Control player = other.GetComponent<Player_Control>();
            if (player != null)
            {
                player.ModifySpeed(0f);
            }
        }
    }
}

