using UnityEngine;

public class CubeRunner : MonoBehaviour
{
    public float forwardSpeed = 5f;  // Speed of forward movement
    public float dragSpeed = 10f;    // Speed of left/right movement

    private Vector3 touchStartPos;
    private Vector3 cubeStartPos;
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
        // Always move forward smoothly
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed);

        // Handle dragging movement
        if (isDragging)
        {
            Vector3 touchDelta = Input.mousePosition - touchStartPos;
            float moveX = touchDelta.x / Screen.width * dragSpeed;

            // Move left/right while keeping forward motion
            rb.velocity = new Vector3(moveX, rb.velocity.y, forwardSpeed);
        }
        else
        {
            // Ensure it goes straight when you release touch
            rb.velocity = new Vector3(0, rb.velocity.y, forwardSpeed);
        }
    }

    void Update()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchStartPos = Input.mousePosition;
            cubeStartPos = rb.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // Stop dragging
        }
    }
}
