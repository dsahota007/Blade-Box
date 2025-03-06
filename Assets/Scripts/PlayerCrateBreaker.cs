using UnityEngine;

namespace DiabolicalGames
{
    public class PlayerCrateBreaker : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToSpawn; // Assign debris prefab in Inspector
        [SerializeField] private GameObject playerMesh;    // Assign the visible player mesh

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            // Proper physics settings
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        void FixedUpdate()
        {
            // Apply movement via Rigidbody to respect physics collision
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 5f); // Keep moving forward
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Cube hit a wall! Normal collision applied.");

                // Stop sideways movement without overriding normal physics
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }

            if (collision.gameObject.CompareTag("Crate"))
            {
                Debug.Log("Player hit a crate! Hiding mesh and spawning debris.");

                // Hide the player's mesh (but keep the player object)
                if (playerMesh != null)
                {
                    playerMesh.SetActive(false);
                    Debug.Log("Player mesh hidden.");
                }

                // Spawn the debris prefab
                if (prefabToSpawn != null)
                {
                    GameObject debris = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                    debris.transform.localScale = transform.localScale;
                    Debug.Log("Spawned prefab: " + prefabToSpawn.name);
                }
            }
        }
    }
}
