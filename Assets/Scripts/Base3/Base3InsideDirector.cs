using System.Collections;
using UnityEngine;

public class Base3InsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string base3OutsideSceneName;
    public string nextSceneName;

    public Transform sitPoint;
    public Transform exitPoint;

    [Header("Engine Audio")]
    public AudioSource engineAudioSource;
    public AudioClip engineLoopClip;
    public AudioClip engineStartClip;

    [Header("Moving Object")]
    public Transform movingObject;
    public Transform objectDestination;
    public float moveSpeed = 1f;

    bool playerExited = false;
    bool returningFromOutside = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerExited = true);

        // Check if coming back from outside
        returningFromOutside = Story.base3OutsideDone;

        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!returningFromOutside)
        {
            // First time in — full intro sequence
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            engineAudioSource.clip = engineLoopClip;
            engineAudioSource.loop = true;
            engineAudioSource.Play();

            StartCoroutine(MoveObject());
            yield return PlayDialogue(0);

            yield return WaitUntilTrue(() => ObjectReachedDestination());

            engineAudioSource.Stop();

            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);

            yield return WaitUntilTrue(() => playerExited);

            GoToScene(base3OutsideSceneName);
        }
        else
        {
            // Returning from outside — sit, engine start, load next scene
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            if (engineStartClip != null)
            {
                PersistentServices.Instance.PlaySFX(engineStartClip);
                yield return new WaitForSeconds(engineStartClip.length);
            }

            engineAudioSource.clip = engineLoopClip;
            engineAudioSource.loop = true;
            engineAudioSource.Play();

            GoToScene(nextSceneName);
        }
    }

    IEnumerator MoveObject()
    {
        while (!ObjectReachedDestination())
        {
            movingObject.position = Vector3.MoveTowards(
                movingObject.position,
                objectDestination.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        movingObject.position = objectDestination.position;
    }

    bool ObjectReachedDestination()
    {
        return Vector3.Distance(movingObject.position, objectDestination.position) < 0.01f;
    }
}