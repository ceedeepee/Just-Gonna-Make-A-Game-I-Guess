using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovementController : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 5f;
    public float minY = -3f; // Make sure player can shoot this low
    public float maxY = 3f; // Make sure player can shoot this high
    public float minXOffset = 2f; // Minimum distance enemy should keep from player
    public float maxXOffset = 5f; // Maximum distance enemy should keep from player

    private Vector3 nextPosition;

    private void Start()
    {
        nextPosition = new Vector3(playerTransform.position.x + maxXOffset, RandomY(), transform.position.z);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, nextPosition) < 0.1f) // This prevents stuttering when the enemy reaches the next position
        {
            // Generate a new position within the allowed y range and ahead of the player
            nextPosition = new Vector3(playerTransform.position.x + RandomXOffset(), RandomY(), transform.position.z);
        }

        // Smoothly move towards the next position
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * speed);
        minY = playerTransform.position.y;
    }

    float RandomY()
    {
        return Random.Range(minY, maxY);
    }

    float RandomXOffset()
    {
        return Random.Range(minXOffset, maxXOffset);
    }
}