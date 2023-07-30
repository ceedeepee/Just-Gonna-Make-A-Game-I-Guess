using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public float spawnInterval = 300f;
    public float initialSpawnDistance = 200f;

    private float nextSpawnDistance;
    private float previousPlayerX;

    void Start()
    {
        nextSpawnDistance = initialSpawnDistance;
    }

    void Update()
    {
        float playerX = playerTransform.position.x;

        if (playerX - previousPlayerX >= nextSpawnDistance)
        {
            // Spawn a new enemy in front of the player
            GameObject robot = Instantiate(enemyPrefab, new Vector3(playerX + 10f, 10f, 0f), Quaternion.identity);
            robot.GetComponent<EnemyMovementController>().playerTransform = playerTransform;
            // Prepare for the next spawn
            previousPlayerX = playerX;
            nextSpawnDistance += spawnInterval;
        }
    }
}