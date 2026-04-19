using UnityEngine;

public class DriveManager : MonoBehaviour
{
    
    // To stop driving, call DriveManager.Instance.StopDriving();
    // To start driving, call DriveManager.Instance.StartDriving();
    
    public static DriveManager Instance;

    public bool isDriving = true;

    [Header("Speed Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 3f;

    public float currentSpeed = 0f;
    private float targetSpeed = 0f;

    private bool hasStopped = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (isDriving)
            targetSpeed = maxSpeed;
    }

    void Update()
    {
        // 🔑 TEST INPUT (press P to toggle driving)
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleDriving();
        }

        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        float rate = (targetSpeed > currentSpeed) ? acceleration : deceleration;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            rate * Time.deltaTime
        );

        // Trigger stop bump once when fully stopped
        if (!isDriving && currentSpeed <= 0.05f)
        {
            currentSpeed = 0f;

            if (!hasStopped)
            {
                hasStopped = true;

                FindFirstObjectByType<TruckJitter>()?.TriggerBump();
            }
        }
        else
        {
            hasStopped = false;
        }
    }

    public void StopDriving()
    {
        isDriving = false;
        targetSpeed = 0f;
    }

    public void StartDriving()
    {
        isDriving = true;
        targetSpeed = maxSpeed;
    }

    void ToggleDriving()
    {
        if (isDriving)
            StopDriving();
        else
            StartDriving();
    }
}