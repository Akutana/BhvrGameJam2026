using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 0.05f;

    private Light flickerLight;
    private float timer;

    void Start()
    {
        flickerLight = GetComponent<Light>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = flickerSpeed;
        }
    }
}