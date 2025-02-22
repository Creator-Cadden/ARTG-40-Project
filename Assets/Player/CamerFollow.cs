using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // The player object to follow
    public float smoothSpeed = 0.125f;  // Smoothness factor (lower values = smoother)
    public Vector3 offset;       // The offset from the player's position (e.g., above the player)

    void LateUpdate()
    {
        // Check if the player is assigned
        if (player != null)
        {
            // The target position is the player's position + the offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }
}