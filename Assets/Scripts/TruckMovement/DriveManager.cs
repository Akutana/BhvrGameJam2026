using UnityEngine;

public class DriveManager : MonoBehaviour
{
    public static DriveManager Instance;

    public bool isDriving = true;
    public float speed = 10f;

    void Awake()
    {
        Instance = this;
    }
}