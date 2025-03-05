using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion; // Assign the broken crate prefab in the Inspector
    private bool hasBroken = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBroken && collision.gameObject.CompareTag("Blade"))
        {
            hasBroken = true;

            GameObject debris = Instantiate(destroyedVersion, transform.position, transform.rotation);

            // **Ensure ALL debris is destroyed**
            StartCoroutine(DestroyDebris(debris, 1f));

            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyDebris(GameObject debris, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (debris != null)
        {
            Debug.Log("Destroying debris: " + debris.name); // Debugging

            foreach (Transform child in debris.transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(debris);
        }

        // **Extra Cleanup: Find & Destroy ALL Crate Debris in the Scene**
        GameObject[] allDebris = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allDebris)
        {
            if (obj.name.Contains("Debris") || obj.name.Contains("Crate"))
            {
                Debug.Log("Force Destroying: " + obj.name); // Debugging
                Destroy(obj);
            }
        }
    }
}
