using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5.0f;

    private Vector2 direction;

    void Update()
    {
        // Move the projectile in the set direction
        transform.position += (Vector3)direction.normalized * speed * Time.deltaTime;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}