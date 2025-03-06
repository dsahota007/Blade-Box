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

        private GameObject debris;
        private Rigidbody rigidbody;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Blade"))
            {
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

            // **Destroy debris prefab after 1 second**
            StartCoroutine(DestroyDebrisAfterTime(debris, 1f));

            // Destroy the original crate immediately
            Destroy(gameObject);
            ScoreManager.instance.AddScore(Random.Range(5, 17));

        }

        private IEnumerator DestroyDebrisAfterTime(GameObject spawnedDebris, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (spawnedDebris != null)
            {
                Debug.Log("Destroying spawned debris: " + spawnedDebris.name);

                // **1. Disable rendering, physics, and colliders**
                foreach (Transform child in spawnedDebris.transform)
                {
                    if (child.TryGetComponent<Renderer>(out Renderer renderer))
                        renderer.enabled = false;
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        rb.isKinematic = true;
                    if (child.TryGetComponent<Collider>(out Collider col))
                        col.enabled = false;
                }

                // **2. Destroy all child objects**
                foreach (Transform child in spawnedDebris.transform)
                {
                    Destroy(child.gameObject);
                }

                // **3. Destroy the debris object itself**
                Debug.Log("Destroying debris prefab: " + spawnedDebris.name);
                Destroy(spawnedDebris);
            }

            // **Extra: Remove ALL debris clones to make sure nothing is left**
            GameObject[] debrisClones = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in debrisClones)
            {
                if (obj.name.Contains("Crate 1 Debris High(Clone)"))
                {
                    Debug.Log("Force Destroying clone: " + obj.name);
                    Destroy(obj);
                }
            }
        }
    }
}
