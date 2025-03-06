using UnityEngine;

namespace DiabolicalGames
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;  // Assign the player (CubePlayer) in the Inspector
        [SerializeField] private Vector3 offset = new Vector3(0, 3, -5); // Default offset
        [SerializeField] private float smoothSpeed = 5f; // Speed of smoothing

        void LateUpdate()
        {
            if (player == null)
            {
                Debug.LogWarning("CameraFollow: Player reference is missing!");
                return;
            }

            // Smoothly move the camera to the target position
            Vector3 desiredPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Keep the camera looking at the player
            transform.LookAt(player);
        }
    }
}
