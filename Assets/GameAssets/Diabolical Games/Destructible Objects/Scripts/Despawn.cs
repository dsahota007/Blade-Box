using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Rajendra Abhinaya, 2023

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
            // Play sound on debris spawn
            audioSource = GetComponent<AudioSource>();
            audioSource.pitch = 1f + Random.Range(-pitchVariation / 2, pitchVariation / 2);
            audioSource.PlayOneShot(clip, volume + Random.Range(-volumeVariation, volumeVariation));
        }

        public void BeginCoroutine(string coroutine)
        {
            switch (coroutine)
            {
                case "Timed":
                    StartCoroutine(DespawnCoroutine());
                    break;
                case "Distance from Player":
                    StartCoroutine(CheckDistance());
                    break;
            }
        }

        public void DespawnDebris()
        {
            int despawnCount = transform.childCount * despawnPercentage / 100;
            for (int i = transform.childCount - 1; i >= transform.childCount - despawnCount; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public IEnumerator CheckDistance()
        {
            yield return new WaitForSeconds(5f);
            while (true)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > distanceFromPlayer)
                {
                    DespawnDebris();
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        public IEnumerator DespawnCoroutine()
        {
            yield return new WaitForSeconds(despawnTime);
            DespawnDebris();
        }
    }
}
