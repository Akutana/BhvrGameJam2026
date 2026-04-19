
using UnityEngine;

public class ChunkMovement : MonoBehaviour
{

    public float speed = 10f;
    public bool isDriving = true;

    void Update()
    {
        if (DriveManager.Instance.isDriving)
        {
            // float speed = DriveManager.Instance.speed; // for the old DriveManager
            float speed = DriveManager.Instance.speed;
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}