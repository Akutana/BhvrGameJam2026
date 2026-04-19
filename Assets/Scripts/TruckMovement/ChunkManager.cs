using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public List<Transform> chunks;

    public float chunkLength = 10f;
    public Transform truck;

    void Update()
    {
        MoveChunks();
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

    void MoveChunks()
    {
        // float speed = DriveManager.Instance.speed; // This is for the old DriveManager
        float speed = DriveManager.Instance.currentSpeed;

        //if (!DriveManager.Instance.isDriving)
        //    return;

        foreach (Transform chunk in chunks)
        {
            chunk.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}