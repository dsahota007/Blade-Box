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

            int debrisIndex = debrisAmount == DebrisAmount.Random ? Random.Range(0, debrisPrefabs.Count) : (int)debrisAmount;
            debris = Instantiate(debrisPrefabs[debrisIndex].prefab, transform.position, Quaternion.identity);
            debris.SetActive(false);
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

            debris.transform.position = transform.position;
            debris.transform.rotation = transform.rotation;
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

            // **Forcefully remove all debris after 1 second**
            StartCoroutine(ForceRemoveAllDebris(1f));

            // Destroy the original object immediately
            Destroy(gameObject);
        }

        private IEnumerator ForceRemoveAllDebris(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (debris != null)
            {
                Debug.Log("Destroying debris: " + debris.name);

                // **1. Disable rendering & physics**
                foreach (Transform child in debris.transform)
                {
                    if (child.TryGetComponent<Renderer>(out Renderer renderer))
                        renderer.enabled = false;
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        rb.isKinematic = true;
                    if (child.TryGetComponent<Collider>(out Collider col))
                        col.enabled = false;
                }

                // **2. Move debris far away**
                debris.transform.position = new Vector3(9999, 9999, 9999);

                // **3. Final Hard Removal**
                DestroyImmediate(debris);
            }

            // **Extra Cleanup: Delete every debris in scene**
            GameObject[] allDebris = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allDebris)
            {
                if (obj.name.Contains("Debris") || obj.name.Contains("Crate"))
                {
                    Debug.Log("Force Destroying: " + obj.name);
                    DestroyImmediate(obj);
                }
            }
        }
    }
}
