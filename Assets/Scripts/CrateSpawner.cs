using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public GameObject cratePrefab; // Assign your crate prefab in the Inspector
    public Transform[] spawnPoints; // Assign spawn points in the Inspector
    public float spawnInterval = 2f; // Time between spawns

    void Start()
    {
        InvokeRepeating("SpawnCrate", 1f, spawnInterval);
    }

    void SpawnCrate()
    {
        if (spawnPoints.Length == 0 || cratePrefab == null)
        {
            Debug.LogError("No spawn points assigned or crate prefab missing!");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn the crate at the chosen spawn point
        Instantiate(cratePrefab, spawnPoint.position, Quaternion.identity);
    }
}
