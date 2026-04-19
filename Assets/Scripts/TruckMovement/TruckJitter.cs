using UnityEngine;

public class TruckJitter : MonoBehaviour
{
    [Header("Base Vibration")]
    public float baseAmplitude = 0.02f;
    public float baseFrequency = 8f;

    [Header("Rotation")]
    public float rotationZAmount = 1.5f;
    public float rotationXAmount = 1.0f;

    [Header("Bumps")]
    public float bumpAmplitude = 0.2f;
    public float bumpDuration = 0.3f;
    public float bumpsPerSecond = 0.5f; // average bumps per second

    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    private float bumpTimer = 0f;

    void Start()
    {
        originalLocalPos = transform.localPosition;
        originalLocalRot = transform.localRotation;
    }

    void Update()
    {
        if (!DriveManager.Instance.isDriving)
            return;

        // Random bump trigger (frame-rate independent)
        if (Random.value < bumpsPerSecond * Time.deltaTime)
        {
            TriggerBump();
        }

        // Base vibration using Perlin noise
        float noiseX = Mathf.PerlinNoise(Time.time * baseFrequency, 0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(0f, Time.time * baseFrequency) - 0.5f;

        Vector3 jitter = new Vector3(noiseX, noiseY, 0f) * baseAmplitude;

        // Big bump effect
        if (bumpTimer > 0f)
        {
            bumpTimer -= Time.deltaTime;

            float strength = bumpAmplitude * (bumpTimer / bumpDuration);
            jitter += Vector3.up * strength;
        }

        // Apply position jitter
        transform.localPosition = originalLocalPos + jitter;

        // Apply rotation jitter
        float rotZ = noiseX * rotationZAmount;
        float rotX = noiseY * rotationXAmount;

        transform.localRotation = originalLocalRot * Quaternion.Euler(rotX, 0f, rotZ);
    }

    public void TriggerBump()
    {
        bumpTimer = bumpDuration;
    }
}