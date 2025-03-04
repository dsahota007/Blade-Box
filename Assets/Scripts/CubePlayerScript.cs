using UnityEngine;

public class CubeRunner : MonoBehaviour
{
    public float forwardSpeed = 5f;  // Speed of forward movement
    public float dragSpeed = 10f;    // Speed of left/right movement

    private Vector3 touchStartPos;   // Stores initial touch/mouse position
    private Vector3 cubeStartPos;    // Stores initial cube position
    private bool isDragging = false; // Track if player is dragging

    void Update()
    {
        // Move forward automatically
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Handle player input (dragging left/right)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchStartPos = Input.mousePosition;
            cubeStartPos = transform.position;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 touchDelta = Input.mousePosition - touchStartPos;
            float moveX = touchDelta.x / Screen.width * dragSpeed; // Normalize movement

            // Move only left or right, keeping Y and Z the same
            Vector3 newPosition = new Vector3(cubeStartPos.x + moveX, transform.position.y, transform.position.z);

            transform.position = newPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
