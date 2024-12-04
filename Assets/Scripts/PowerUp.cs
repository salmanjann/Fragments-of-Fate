using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject objectPrefab; // Reference to the prefab
    private Vector3 startPosition; // Position to respawn

    void Start()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object destroyed"); // Debugging
        Destroy(gameObject);

        StartCoroutine(RespawnObject());
    }

    IEnumerator RespawnObject()
    {
        Debug.Log("Respawn initiated...");
        yield return new WaitForSeconds(5);
        if (objectPrefab != null)
        {
            Instantiate(objectPrefab, startPosition, Quaternion.identity);
            Debug.Log("Object respawned");
        }
        else
        {
            Debug.LogError("objectPrefab is not assigned in the Inspector!");
        }
    }
}
