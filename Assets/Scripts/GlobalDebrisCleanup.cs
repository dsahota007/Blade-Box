using UnityEngine;
using System.Collections;

public class GlobalDebrisCleanup : MonoBehaviour
{
    private void Update()
    {
        // Find all newly spawned debris
        GameObject[] allDebris = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject debris in allDebris)
        {
            if (debris.name.Contains("Crate 1 Debris High(Clone)"))
            {
                // Start the destruction timer if not already running
                if (!debris.GetComponent<DebrisDestroyTimer>())
                {
                    debris.AddComponent<DebrisDestroyTimer>();
                }
            }
        }
    }
}

// **Helper Script: Destroys debris after 2 seconds**
public class DebrisDestroyTimer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(2f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Destroying debris: " + gameObject.name);
        Destroy(gameObject);
    }
}
