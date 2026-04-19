using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public float stepInterval = 2.0f;
    public PlayerController playerController;

    [Header("Surface Sounds")]
    public AudioClip[] snowSteps;
    public AudioClip[] metalSteps;
    public AudioClip[] defaultSteps;
    public AudioClip[] ladderSteps;

    private float timer;
    private bool wasMoving = false;
    private string currentSurfaceTag = "Untagged";

    void Update()
    {
        bool isClimbing = playerController != null && playerController.currentState == PlayerController.State.Climbing;

        bool isMoving = IsMovingInput();

        if (isMoving && !wasMoving)
        {
            PlayFootstep(isClimbing);
            timer = stepInterval;
        }

        if (isMoving)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                PlayFootstep(isClimbing);
                timer = stepInterval;
            }
        }

        if (!isMoving && wasMoving)
            timer = 0f;

        wasMoving = isMoving;

        // Only raycast when not climbing
        if (!isClimbing)
            DetectSurface();
    }

    void DetectSurface()
    {
        Vector3 origin = playerController.GetComponent<CharacterController>().bounds.center;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f))
            currentSurfaceTag = hit.collider.tag;
    }

    bool IsMovingInput()
    {
        return Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) ||
               Input.GetKey(KeyCode.D);
    }

    void PlayFootstep(bool isClimbing)
    {
        AudioClip[] clips;

        if (isClimbing)
        {
            clips = ladderSteps;
        }
        else
        {
            clips = currentSurfaceTag switch
            {
                "Snow" => snowSteps,
                "Metal" => metalSteps,
                _ => defaultSteps
            };
        }

        if (clips == null || clips.Length == 0) return;

        var clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}