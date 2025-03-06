using UnityEngine;

public class CubeRunner : MonoBehaviour
{
    public float dragSpeed = 10f;    // Speed of left/right movement

    private Vector3 touchStartPos;
    private bool isDragging = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Proper Rigidbody settings for smooth physics
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 touchDelta = Input.mousePosition - touchStartPos;
            float moveX = touchDelta.x / Screen.width * dragSpeed;

            // Apply only left/right movement, without affecting the vertical (y-axis) or z-axis
            rb.velocity = new Vector3(moveX, rb.velocity.y, 0);
        }
        else
        {
            // Ensure it stays in place when not dragging
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Update()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // Stop dragging
        }
    }
}
