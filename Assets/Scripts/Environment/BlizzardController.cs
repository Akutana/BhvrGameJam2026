using UnityEngine;

public class BlizzardController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem snow;

    [Header("Blizzard Intensity")]
    [Range(0f, 1f)]
    public float intensity = 0f;

    [Header("Fog Settings")]
    public float minFogDensity = 0.05f;
    public float maxFogDensity = 0.2f;

    public Color lightFogColor = new Color(0.7f, 0.7f, 0.7f);
    public Color heavyFogColor = new Color(0.784f, 0.784f, 0.784f);

    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.VelocityOverLifetimeModule velocity;
    private ParticleSystem.NoiseModule noise;
    private ParticleSystem.MainModule main;

    void Start()
    {
        emission = snow.emission;
        velocity = snow.velocityOverLifetime;
        noise = snow.noise;
        main = snow.main;
    }

    void Update()
    {
        UpdateSnow();
        UpdateFog();
    }

    void UpdateSnow()
    {
        // Density
        emission.rateOverTime = Mathf.Lerp(200f, 1200f, intensity);

        // Falling speed
        velocity.y = Mathf.Lerp(-1f, -4f, intensity);

        // Wind (sideways force)
        velocity.x = Mathf.Lerp(0f, 8f, intensity);
        velocity.z = Mathf.Lerp(0f, 4f, intensity);

        // Chaos / turbulence
        noise.strength = Mathf.Lerp(0.5f, 4f, intensity);

        // Particle size
        main.startSize = Mathf.Lerp(0.05f, 0.15f, intensity);

        // Lifetime (longer in storm = denser feel)
        main.startLifetime = Mathf.Lerp(5f, 10f, intensity);
    }

    void UpdateFog()
    {
        RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, intensity);
        RenderSettings.fogColor = Color.Lerp(lightFogColor, heavyFogColor, intensity);
    }
}