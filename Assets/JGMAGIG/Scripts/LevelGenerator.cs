using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> levelChunks; // assign in Inspector
    public int poolSize = 5; // assign in Inspector
    private PlayerController playerController;
    private Vector3 nextSpawnPoint;
    private Queue<GameObject> chunkPool = new Queue<GameObject>();
    public List<GameObject> obstaclePrefabs; // Assign this in the inspector
    public float minObstacleX = 1f; // Minimum x position for obstacle spawn
    public float maxObstacleX = 5f; // Maximum x position for obstacle spawn
    public float chunkHeight; // Assign in Inspector
    private Transform playerTransform;

    void Start()
    { 
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject chunk = Instantiate(levelChunks[Random.Range(0, levelChunks.Count)]);
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
            chunk.transform.parent = transform;
        }

        for (int i = 0; i < poolSize; i++)
        {
            SpawnLevelChunk();
        }
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private int runLength = 0;
    private int direction = 1;
    void Update()
    {
        // Get the player's position
        float playerX = playerTransform.position.x;

        // If the player is close to the end of the current chunks, spawn a new one
        if (nextSpawnPoint.x - playerX < 20f)  // You can adjust this value as needed
        {
            if (chunkPool.Count > 0)
            {
                SpawnLevelChunk();
            }
            else
            {
                Debug.Log("Chunk pool is empty, cannot spawn new chunk.");
            }
        }

        // Check each active chunk in the pool
        GameObject[] chunks = chunkPool.ToArray();  // Get a non-iterating collection
        for (int i = chunks.Length - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];
            // If the player is far enough from the first chunk in the queue, recycle it
            if (chunk.activeInHierarchy && chunk.transform.position.x + chunk.GetComponentInChildren<Renderer>().bounds.size.x < playerX - 50f)  // You can adjust this value as needed
            {
                // Recycle the chunk
                RecycleChunk(chunk);
            }
        }

        // Decrease runLength by 1 for each frame
        if(runLength > 0)
        {
            runLength--;
        }
    }

    void RecycleChunk(GameObject chunk)
    {
        chunk.SetActive(false);
        chunkPool.Enqueue(chunk);
    }

    
    public void SpawnLevelChunk()
    {
        GameObject chunk = chunkPool.Dequeue();

        // Calculate the maximum jump distance
        float maxJumpDistance = playerController.MaxJumpDistance();

        // Calculate a difficulty factor based on the distance traveled
        float difficulty = playerController.DistanceTraveled() / 1000f;  // Adjust the denominator as needed

        // Limit the difficulty factor to a maximum of 1
        difficulty = Mathf.Min(difficulty, 1f);

        // Base the gap size on the maximum jump distance and the difficulty factor
        float gap = Random.Range(0, maxJumpDistance * difficulty);

        // Introduce a variation in y position after 20 meters, within the player's jump range
        if (nextSpawnPoint.x > 20)
        {
            // Decrease runLength by 1 for each new chunk
            if (runLength <= 0)
            {
                runLength = Random.Range(3, 7);  // Adjust these numbers as needed to control flat run length
                direction = ChooseDirection();
            }

            float newY;
            if (direction == -1) {
                // For downward movement, allow more variation
                newY = direction * Random.Range(1, 3) * chunkHeight;
            } else if (direction == 1) {
                // For upward movement, limit to the player's maximum jump height
                newY = direction * chunkHeight;
            } else {
                // For flat movement, keep newY as zero
                newY = 0;
            }

            // Calculate potential new Y position
            float potentialNewY = nextSpawnPoint.y + newY;

            // Check if the potential new Y position is within allowed range
            if(potentialNewY < minY || potentialNewY > maxY)
            {
                // If not within range, force a flat run
                direction = 0;
                newY = 0;
            }

            nextSpawnPoint += new Vector3(gap, newY, 0);
        }


        else
        {
            nextSpawnPoint += new Vector3(gap, 0, 0); // For the first 20 meters, keep the y position constant
        }

        chunk.transform.position = nextSpawnPoint;
        chunk.SetActive(true);

        // Update nextSpawnPoint based on current chunk's position and its size
        float chunkWidth = chunk.GetComponentInChildren<Renderer>().bounds.size.x;
        nextSpawnPoint = new Vector3(chunk.transform.position.x + chunkWidth, chunk.transform.position.y, chunk.transform.position.z);

        SpawnObstacles(chunk);

        chunkPool.Enqueue(chunk);
    }
    int minY = -3;
    int maxY = 3;
    int ChooseDirection()
    {
        float randomValue = Random.value;

        if (randomValue < 0.5f) // 50% chance for flat run
            return 0;
        else if (randomValue < 0.75f) // 25% chance for going up
            return 1;
        else // 25% chance for going down
            return -1;
    }
    // public void RecycleChunk(GameObject chunk)
    // {
    //     chunk.SetActive(false);
    // }
public float spawnChanceStart = 0.2f; // assign in Inspector
    private void SpawnObstacles(GameObject chunk)
    {
        // Don't spawn obstacles within the first 20 meters
        if (nextSpawnPoint.x < 20f) return;

        // Calculate the progress made in the game so far. This will be a value between 0 (at the start of the game)
        // and 1 (after the player has moved 1000 units in the x direction).
        float progress = Mathf.Clamp(nextSpawnPoint.x / 5000f, 0f, 1f);

        // Use the progress to calculate the spawn chance, ramping up from 0.5 at the start of the game to 0.75 after 1000 units.
        float spawnChance = Mathf.Lerp(spawnChanceStart, 0.75f, progress);

        // Randomly decide if to spawn an obstacle based on the spawn chance.
        if (Random.value > spawnChance) return;

        // Choose a random obstacle prefab.
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

        // Calculate the position to spawn the obstacle
        Vector3 spawnPosition = CalculateObstaclePosition(chunk, obstaclePrefab);

        // Instantiate the obstacle at the calculated position
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // Make the obstacle a child of the chunk, so it will automatically move and be recycled with the chunk.
        obstacle.transform.parent = chunk.transform;
    }


    private Vector3 CalculateObstaclePosition(GameObject chunk, GameObject obstaclePrefab)
    {
        // Get the bounds of the chunk and the obstacle
        Bounds chunkBounds = chunk.GetComponentInChildren<Renderer>().bounds;
        Bounds obstacleBounds = obstaclePrefab.GetComponentInChildren<Renderer>().bounds;

        // Calculate the y position to place the obstacle on top of the chunk
        float yPos = chunkBounds.max.y + obstacleBounds.extents.y;

        // Use the x position of the chunk plus some random offset within its width
        float xPos = chunkBounds.min.x + Random.Range(0, chunkBounds.size.x);

        return new Vector3(xPos, yPos, chunk.transform.position.z);
    }
}
