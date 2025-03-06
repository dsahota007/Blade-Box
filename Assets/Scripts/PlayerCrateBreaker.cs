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

        void OnCollisionEnter(Collision collision)
        {
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

                // **Trigger the Death Screen using GameManager**
                if (GameManager.instance != null)
                {
                    GameManager.instance.ShowDeathScreen();
                }

                // Optionally disable the player object
                gameObject.SetActive(false);
            }
        }
    }
}
