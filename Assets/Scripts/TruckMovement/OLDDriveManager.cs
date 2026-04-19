using UnityEngine;

public class OLDDriveManager : MonoBehaviour
{
    
    // To stop the driving, call DriveManager.Instance.StopDriving();
    public static OLDDriveManager Instance;

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

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleDriving();
        }
    }

    void ToggleDriving()
    {
        if (isDriving)
            StopDriving();
        else
            StartDriving();
    }
}