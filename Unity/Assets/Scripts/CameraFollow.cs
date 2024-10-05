using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Adjust this to control how smooth the camera follows
    public Vector3 offset; // Offset from the player position

    private void FixedUpdate()
    {
        // Desired position of the camera, but keep z axis constant
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = transform.position.z; // Keep the camera's z position constant

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Keep the camera from rotating with the player
        transform.rotation = Quaternion.identity;
    }
}