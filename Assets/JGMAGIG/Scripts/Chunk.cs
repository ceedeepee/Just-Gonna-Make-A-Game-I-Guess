using UnityEngine;

public class Chunk : MonoBehaviour
{
    private LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
    }

    // void OnBecameInvisible()
    // {
    //     //levelGenerator.RecycleChunk(gameObject);
    //     levelGenerator.SpawnLevelChunk();
    // }
}
