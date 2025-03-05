using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Rajendra Abhinaya, 2023

namespace DiabolicalGames
{
    public class DestructibleObject : MonoBehaviour
    {
        enum DebrisAmount
        {
            Low,
            Medium,
            High,
            Random
        }

        enum DespawnType
        {
            None,
            Timed,
            DistanceFromPlayer
        }

        [System.Serializable]
        struct DebrisPrefab
        {
            public string name;
            public GameObject prefab;
        }

        [Header("Debris")]
        [SerializeField] private List<DebrisPrefab> debrisPrefabs = new List<DebrisPrefab>();
        [SerializeField] private DebrisAmount debrisAmount = new DebrisAmount();
        [SerializeField] private float forceRequired;

        [Header("Despawning")]
        [SerializeField] private DespawnType despawnType = new DespawnType();
        [SerializeField, Range(0, 100)] private int despawnPercentage;
        [SerializeField] private float despawnTime;
        [SerializeField] private GameObject player;
        [SerializeField] private float distanceFromPlayer;

        [Header("Audio")]
        [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
        [SerializeField, Range(0f, 1f)] private float volume;
        [SerializeField, Range(0f, 0.2f)] private float volumeVariation;
        [SerializeField, Range(0f, 0.5f)] private float pitchVariation;

        private GameObject debris;
        private Rigidbody rigidbody;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();

            // 🚀 **Fix 1: Make Rigidbody kinematic at the start so it doesn't react to gravity or other forces**
            rigidbody.isKinematic = true;

            // Instantiate debris prefab
            int debrisIndex = debrisAmount == DebrisAmount.Random ? Random.Range(0, debrisPrefabs.Count) : (int)debrisAmount;
            debris = Instantiate(debrisPrefabs[debrisIndex].prefab, transform.position, Quaternion.identity);
            debris.SetActive(false);
        }

        void OnCollisionEnter(Collision collision)
        {
            // 🚀 **Fix 2: Only break when colliding with "Blade"**
            if (collision.gameObject.CompareTag("Blade"))
            {
                Break();
            }
        }

        public void Break()
        {
            // 🚀 **Fix 3: Enable Rigidbody physics only when breaking**
            rigidbody.isKinematic = false;

            // Set debris properties
            debris.transform.position = transform.position;
            debris.transform.rotation = transform.rotation;
            debris.transform.localScale = transform.localScale;
            debris.SetActive(true);

            // Apply force to debris
            foreach (Transform child in debris.transform)
            {
                Rigidbody debrisRigidbody = child.GetComponent<Rigidbody>();
                if (debrisRigidbody != null)
                {
                    Vector3 randomForce = Random.insideUnitSphere * 2;
                    debrisRigidbody.velocity = rigidbody.velocity + randomForce;
                }
            }

            // Activate despawn mechanism
            var despawnComponent = debris.GetComponent<Despawn>();
            if (despawnComponent != null)
            {
                despawnComponent.SetVariables(despawnPercentage, despawnTime, distanceFromPlayer, player, audioClips[Random.Range(0, audioClips.Count)], volume, volumeVariation, pitchVariation);

                if (despawnType == DespawnType.Timed)
                    despawnComponent.BeginCoroutine("Timed");
                else if (despawnType == DespawnType.DistanceFromPlayer)
                    despawnComponent.BeginCoroutine("Distance from Player");
            }

            // Destroy the main object
            Destroy(gameObject);
        }
    }
}
