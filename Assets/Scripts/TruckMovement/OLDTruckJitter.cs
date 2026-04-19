using UnityEngine;

public class OLDTruckJitter : MonoBehaviour
{
    public float baseAmplitude = 0.02f;  // 0.01 - 0.03
    public float baseFrequency = 8f;    // 6 - 12

    public float bumpAmplitude = 0.2f;
    public float bumpDuration = 0.3f;

    private Vector3 originalLocalPos;
    private float bumpTimer = 0f;

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (!DriveManager.Instance.isDriving)
            return;

        // Random bump trigger
        // To manually call a "big bump", use GetComponent<TruckJitter>().TriggerBump();
        if (Random.value < 0.005f)
        {
            TriggerBump();
        }

        // Base small vibration
        float noiseX = Mathf.PerlinNoise(Time.time * baseFrequency, 0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(0f, Time.time * baseFrequency) - 0.5f;

        Vector3 jitter = new Vector3(noiseX, noiseY, 0f) * baseAmplitude;

        // Big bump (if active)
        if (bumpTimer > 0f)
        {
            bumpTimer -= Time.deltaTime;

            float bumpStrength = bumpAmplitude * (bumpTimer / bumpDuration);
            jitter += Vector3.up * bumpStrength;
        }

        transform.localPosition = originalLocalPos + jitter;
    }

    public void TriggerBump()
    {
        bumpTimer = bumpDuration;
    }
}