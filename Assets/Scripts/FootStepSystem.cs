using UnityEngine;

public enum SurfaceType
{
    Snow,
    Metal,
    Default
}

public class FootstepSystem : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] snowSteps;
    public float stepInterval = 2.0f;

    private float timer;
    private bool wasMoving = false;

    void Update()
    {
        bool isMoving = IsMovingInput();

        // START moving
        if (isMoving && !wasMoving)
        {
            PlayFootstep();
            timer = stepInterval;
        }

        // WHILE moving
        if (isMoving)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                PlayFootstep();
                timer = stepInterval;
            }
        }

        // STOP moving
        if (!isMoving && wasMoving)
        {
            timer = 0f; // prevent delayed extra step
        }

        wasMoving = isMoving;
    }

    bool IsMovingInput()
    {
        return Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) ||
               Input.GetKey(KeyCode.D);
    }

    void PlayFootstep()
    {
        if (snowSteps.Length == 0) return;

        var clip = snowSteps[Random.Range(0, snowSteps.Length)];

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}