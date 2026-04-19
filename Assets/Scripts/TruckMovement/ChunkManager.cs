using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public List<Transform> chunks;

    public float chunkLength = 100f;
    public Transform truck;

    void Update()
    {
        RecycleChunks();
    }

    void RecycleChunks()
    {
        foreach (Transform chunk in chunks)
        {
            // If chunk is far behind the truck
            if (chunk.position.z < truck.position.z - chunkLength)
            {
                MoveChunkToFront(chunk);
            }
        }
    }

    void MoveChunkToFront(Transform chunk)
    {
        float maxZ = GetMaxChunkZ();
        chunk.position = new Vector3(0, 0, maxZ + chunkLength);
    }

    float GetMaxChunkZ()
    {
        float maxZ = float.MinValue;

        foreach (Transform chunk in chunks)
        {
            if (chunk.position.z > maxZ)
                maxZ = chunk.position.z;
        }

        return maxZ;
    }
}