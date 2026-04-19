using System.Collections;
using UnityEngine;

public class FinalInsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string finalOutsideSceneName;

    public Transform sitPoint;
    public Transform exitPoint;

    [Header("Engine Audio")]
    public AudioSource engineAudioSource;
    public AudioClip engineLoopClip;
    public AudioClip breakingNoiseClip;
    public AudioClip stallingNoiseClip;
    public AudioClip warningClip;

    [Header("Lights")]
    public Light[] flickeringLights;

    [Header("Moving Object")]
    public Transform movingObject;
    public Transform objectDestination;
    public float moveSpeed = 1f;

    bool playerExited = false;
    bool objectReached = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerExited = true);

        // Start flickering all lights
        foreach (Light l in flickeringLights)
            l.GetComponent<LightFlicker>().enabled = true;

        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        // Sit player
        player.Sit(sitPoint, exitPoint);
        player.forcedToSit = true;

        // Start engine loop
        engineAudioSource.clip = engineLoopClip;
        engineAudioSource.loop = true;
        engineAudioSource.Play();

        // Dialogue
        yield return PlayDialogue(0);

        // Warning sound
        PersistentServices.Instance.PlaySFX(warningClip);

        // Move object from A to B - don't wait for it to finish yet
        StartCoroutine(MoveObject());

        // Wait for object to reach destination
        yield return WaitUntilTrue(() => objectReached);

        // TODO: call function here (object reached destination)

        // Breaking noise and cut engine
        PersistentServices.Instance.PlaySFX(breakingNoiseClip);
        engineAudioSource.Stop();

        // TODO: call function here to turn lights off
        foreach (Light l in flickeringLights)
        {
            l.GetComponent<LightFlicker>().enabled = false;
            l.enabled = false;
        }

        // Start stalling noise and play dialogue at the same time
        engineAudioSource.clip = stallingNoiseClip;
        engineAudioSource.loop = true;
        engineAudioSource.Play();

        yield return PlayDialogue(1);

        // Dialogue done - player can leave
        engineAudioSource.Stop();
        player.forcedToSit = false;
        truckDoor.setCanEnterDoor(true);

        yield return WaitUntilTrue(() => playerExited);

        GoToScene(finalOutsideSceneName);
    }

    IEnumerator MoveObject()
    {
        while (Vector3.Distance(movingObject.position, objectDestination.position) > 0.01f)
        {
            movingObject.position = Vector3.MoveTowards(
                movingObject.position,
                objectDestination.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        movingObject.position = objectDestination.position;
        objectReached = true;
    }
}