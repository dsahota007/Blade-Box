using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiabolicalGames
{
    public class Despawn : MonoBehaviour
    {
        private int despawnPercentage;
        private float despawnTime;
        private float distanceFromPlayer;
        private GameObject player;
        private AudioClip clip;
        private float volume;
        private float volumeVariation;
        private float pitchVariation;
        private AudioSource audioSource;

        private bool hasBeenHit = false; // Prevents breaking until Blade hits

        public void SetVariables(int despawnPercentage, float despawnTime, float distanceFromPlayer, GameObject player, AudioClip clip, float volume, float volumeVariation, float pitchVariation)
        {
            this.despawnPercentage = despawnPercentage;
            this.despawnTime = despawnTime;
            this.distanceFromPlayer = distanceFromPlayer;
            this.player = player;
            this.clip = clip;
            this.volume = volume;
            this.volumeVariation = volumeVariation;
            this.pitchVariation = pitchVariation;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource != null && clip != null)
            {
                audioSource.pitch = 1f + Random.Range(-pitchVariation / 2, pitchVariation / 2);
                audioSource.PlayOneShot(clip, volume + Random.Range(-volumeVariation, volumeVariation));
            }

            // Make sure the debris doesn't move until hit
            FreezeDebris();
        }

        // Freezes all debris so it doesn’t break on spawn
        private void FreezeDebris()
        {
            foreach (Transform child in transform)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Prevents movement
                    rb.useGravity = false; // Stops falling
                }
            }
        }

        // Unfreezes debris, allowing them to collapse
        private void UnfreezeDebris()
        {
            foreach (Transform child in transform)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false; // Enable physics
                    rb.useGravity = true;
                }
            }
        }

        // ✅ Now it only despawns debris when hit by Blade
        public void DespawnDebris()
        {
            if (!hasBeenHit) return; // Only break if hit

            int despawnCount = transform.childCount * despawnPercentage / 100;
            for (int i = transform.childCount - 1; i >= transform.childCount - despawnCount; i--)
            {
                if (i >= 0)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
        }

        // 💥 Only break when hit by the Blade
        private void OnCollisionEnter(Collision collision)
        {
            if (!hasBeenHit && collision.gameObject.CompareTag("Blade"))
            {
                hasBeenHit = true;
                UnfreezeDebris(); // Enable physics when hit by Blade
                StartCoroutine(DelayedDespawn()); // Add a delay before fully despawning
            }
        }

        // 📌 Adds a slight delay before debris disappears
        private IEnumerator DelayedDespawn()
        {
            yield return new WaitForSeconds(3f); // Adjust as needed
            DespawnDebris();
        }
    }
}
