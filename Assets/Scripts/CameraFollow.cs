using UnityEngine;

namespace DiabolicalGames
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;  // Assign the CubePlayer in the Inspector
        [SerializeField] private Vector3 offset = new Vector3(0, 4, -8); // Adjust this manually
        [SerializeField] private float smoothSpeed = 5f; // Smoothing factor

        void LateUpdate()
        {
            if (player == null) return;

            // Follow the player with an offset
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
