using UnityEngine;

namespace DiabolicalGames
{
    public class PlayerCrateBreaker : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToSpawn; // Assign debris prefab in Inspector
        [SerializeField] private GameObject playerMesh;    // Assign the visible player mesh

        [Header("Audio")]
        [SerializeField] private AudioClip deathSound; // Drag the player death sound in the Inspector

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

                // Hide the player's mesh (but keep the player object)
                if (playerMesh != null)
                {
                    playerMesh.SetActive(false);

                }

                // Spawn the debris prefab
                if (prefabToSpawn != null)
                {
                    GameObject debris = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                    debris.transform.localScale = transform.localScale;
                }

                // ✅ Play Death Sound
                PlayDeathSound();

                // ✅ Show Final Score and High Score before death screen
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.ShowFinalScore(); // Display final score and high score
                }

                // ✅ Trigger the Death Screen using GameManager
                if (GameManager.instance != null)
                {
                    GameManager.instance.ShowDeathScreen(); // Activate death screen
                }

                // ✅ Optionally disable the player object
                gameObject.SetActive(false);
            }
        }

        private void PlayDeathSound()
        {
            if (deathSound != null)
            {
                // **Create a separate object to play the sound**
                GameObject soundObject = new GameObject("PlayerDeathSound");
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();

                audioSource.clip = deathSound;
                audioSource.Play();

                // Destroy the sound object after it finishes playing
                Destroy(soundObject, audioSource.clip.length);
            }
        }
    }
}
