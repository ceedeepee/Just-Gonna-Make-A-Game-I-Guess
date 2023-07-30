using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // assign in Inspector
    public float fireRate = 2.0f; // time between shots
    private GameObject player;
    private float nextFire;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nextFire = Time.time;
        if (Vector3.Distance(transform.position, Vector3.zero) < 20)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Check if it's time to fire
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        if (player != null)
        {
            // Instantiate a new projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Calculate direction towards player
            Vector2 direction = player.transform.position - transform.position;

            // Set the projectile's direction
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }
    }    
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Check if collided with a projectile
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //             Destroy(gameObject);
    //     }
    // }
}