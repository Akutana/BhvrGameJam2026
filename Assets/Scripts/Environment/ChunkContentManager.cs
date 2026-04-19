using System.Collections.Generic;
using UnityEngine;

public class ChunkContentManager : MonoBehaviour
{
    public static ChunkContentManager Instance;

    [Header("Spawning Control")]
    public bool spawnObstacles = true;

    [Header("Obstacle Prefabs")]
    public List<GameObject> obstaclePrefabs;

    [Header("Placement Settings")]
    public float lateralRange = 4f;
    public float forwardRange = 4f;

    [Header("Height Fix")]
    public float spawnHeight = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnForChunk(Transform chunk)
    {
        if (!spawnObstacles)
            return;

        if (obstaclePrefabs == null || obstaclePrefabs.Count == 0)
            return;

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

        Vector3 offset = new Vector3(
            Random.Range(-lateralRange, lateralRange),
            spawnHeight,
            Random.Range(-forwardRange, forwardRange)
        );

        Instantiate(
            prefab,
            chunk.position + offset,
            Quaternion.identity,
            chunk
        );
    }

    public void StopSpawningObstacles()
    {
        spawnObstacles = false;
    }

    public void StartSpawningObstacles()
    {
        spawnObstacles = true;
    }
}