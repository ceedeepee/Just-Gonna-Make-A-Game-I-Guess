using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int hitsToDestroy = 1;
    private int hitCount = 0;
    public GameObject explosionPrefab;
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided with a projectile
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerController>().objectsBlownUp++;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
//            Debug.Log("Hit by projectile!");
            hitCount++;

            if (hitCount >= hitsToDestroy)
            {
                Destroy(gameObject);
            }
            if (!collision.gameObject.CompareTag("Player")){
                Destroy(collision.gameObject);
            }
        }
    }

    public void blowUp()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
//            Debug.Log("Hit by projectile!");
        hitCount++;

        if (hitCount >= hitsToDestroy)
        {
            Destroy(gameObject);
        }
        FindObjectOfType<PlayerController>().objectsBlownUp++;
    }
}