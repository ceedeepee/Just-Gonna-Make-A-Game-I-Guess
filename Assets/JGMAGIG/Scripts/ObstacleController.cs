using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject destroyedVersion; // Assign in inspector
    public GameObject spawnVersion;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Instantiate a destroyed version of the obstacle
            destroyedVersion.SetActive(true);

            // Disable the original obstacle
            spawnVersion.SetActive(false);

            // Optional: You could start a coroutine here to destroy the "destroyed version" after a delay, e.g.:
            StartCoroutine(DestroyAfterDelay(gameObject, 5f));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}