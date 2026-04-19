using UnityEngine;

public class LightButtonInteractable : Interactable
{
    private bool lightsOn = true;

    private Light[] lights;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Toggle");
        lights = new Light[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            lights[i] = objs[i].GetComponent<Light>();
        }
    }

    public override void Interact()
    {
        lightsOn = !lightsOn;

        ToggleLights(lightsOn);
    }

    public void ToggleLights(bool lightsOn)
    {
        foreach (Light light in lights)
        {
            if (light != null)
                light.enabled = lightsOn;
        }
    }
}
