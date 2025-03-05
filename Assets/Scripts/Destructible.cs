using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion; // Assign the broken crate prefab in the Inspector
    private bool hasBroken = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Break only once when hit by an object tagged "Blade"
        if (!hasBroken && collision.gameObject.CompareTag("Blade"))
        {
            hasBroken = true;

            // Spawn the broken debris
            GameObject debris = Instantiate(destroyedVersion, transform.position, transform.rotation);

            // Destroy all child objects and components after 1 second
            StartCoroutine(DestroyCompletely(debris, 1f));

            // Destroy the original crate immediately
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator DestroyCompletely(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            // Destroy all child objects
            foreach (Transform child in obj.transform)
            {
                Destroy(child.gameObject);
            }

            // Destroy any remaining components
            foreach (Component comp in obj.GetComponents<Component>())
            {
                if (!(comp is Transform)) // Keep the Transform to destroy the GameObject last
                {
                    Destroy(comp);
                }
            }

            // Finally, destroy the object itself
            Destroy(obj);
        }
    }
}
