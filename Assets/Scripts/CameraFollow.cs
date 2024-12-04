using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform
    public Vector3 offset;    // Offset from the player's position
    public float smoothSpeed = 0.125f;  // Smoothing speed

    private float fixedZ;  // The constant Z value of the camera

    void Start()
    {
        fixedZ = transform.position.z;  // Save the camera's initial Z value
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate desired position, keeping the Z-axis fixed
            Vector3 desiredPosition = player.position + offset;
            desiredPosition.z = fixedZ;  // Lock Z-axis

            // Smoothly interpolate camera's position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
