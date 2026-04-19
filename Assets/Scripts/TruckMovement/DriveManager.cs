using UnityEngine;

public class DriveManager : MonoBehaviour
{
    
    // To stop the driving, call DriveManager.Instance.StopDriving();
    public static DriveManager Instance;

    public bool isDriving = true;

    public float speed = 10f;
    private float defaultSpeed;

    void Awake()
    {
        Instance = this;
        defaultSpeed = speed;
    }

    public void StopDriving()
    {
        isDriving = false;
        speed = 0f;
    }

    public void StartDriving()
    {
        isDriving = true;
        speed = defaultSpeed;
    }
}