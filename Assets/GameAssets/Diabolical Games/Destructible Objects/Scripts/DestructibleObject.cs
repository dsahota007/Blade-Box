using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiabolicalGames
{
    public class DestructibleObject : MonoBehaviour
    {
        enum DebrisAmount { Low, Medium, High, Random }

        [System.Serializable]
        struct DebrisPrefab
        {
            public string name;
            public GameObject prefab;
        }

        [Header("Debris")]
        [SerializeField] private List<DebrisPrefab> debrisPrefabs = new List<DebrisPrefab>();
        [SerializeField] private DebrisAmount debrisAmount = new DebrisAmount();

        [Header("Audio")]
        [SerializeField] private AudioClip[] woodSounds; // Drag your audio clips here

        private GameObject debris;
        private Rigidbody rigidbody;
        private bool isBroken = false; // Prevent multiple triggers

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Blade") && !isBroken)
            {
                isBroken = true;
                Break();
            }
        }

        public void Break()
        {
            rigidbody.isKinematic = false;

            // **Spawn High Debris**
            int debrisIndex = debrisAmount == DebrisAmount.Random ? Random.Range(0, debrisPrefabs.Count) : (int)debrisAmount;
            debris = Instantiate(debrisPrefabs[debrisIndex].prefab, transform.position, Quaternion.identity);
            debris.transform.localScale = transform.localScale;
            debris.SetActive(true);

            foreach (Transform child in debris.transform)
            {
                Rigidbody debrisRigidbody = child.GetComponent<Rigidbody>();
                if (debrisRigidbody != null)
                {
                    debrisRigidbody.velocity = rigidbody.velocity + Random.insideUnitSphere * 2;
                }
            }

            // **Play Random Sound**
            PlayRandomSound();

            // **Destroy debris prefab after 1 second**
            StartCoroutine(DestroyDebrisAfterTime(debris, 1f));

            // Destroy the original crate immediately
            Destroy(gameObject);
            ScoreManager.instance.AddScore(Random.Range(5, 17));
        }

        private void PlayRandomSound()
        {
            if (woodSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, woodSounds.Length);

                // **Create a separate object to play the sound**
                GameObject soundObject = new GameObject("CrateSound");
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();

                audioSource.clip = woodSounds[randomIndex];
                audioSource.Play();

                // **Destroy the sound object after clip finishes**
                Destroy(soundObject, audioSource.clip.length);
            }
        }

        private IEnumerator DestroyDebrisAfterTime(GameObject spawnedDebris, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (spawnedDebris != null)
            {
                foreach (Transform child in spawnedDebris.transform)
                {
                    if (child.TryGetComponent<Renderer>(out Renderer renderer))
                        renderer.enabled = false;
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        rb.isKinematic = true;
                    if (child.TryGetComponent<Collider>(out Collider col))
                        col.enabled = false;
                }

                foreach (Transform child in spawnedDebris.transform)
                {
                    Destroy(child.gameObject);
                }

                Destroy(spawnedDebris);
            }

            // **Cleanup remaining debris clones**
            GameObject[] debrisClones = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in debrisClones)
            {
                if (obj.name.Contains("Crate 1 Debris High(Clone)"))
                {
                    Destroy(obj);
                }
            }
        }
    }
}
