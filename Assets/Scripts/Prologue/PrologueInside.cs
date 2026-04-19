using System.Collections;
using UnityEngine;

public class PrologueInsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string prologueOutsideSceneName;
    public string nextSceneName;

    public Transform sitPoint;
    public Transform exitPoint;

    [Header("Engine Audio")]
    public AudioSource engineAudioSource;
    public AudioClip engineStartClip;
    public AudioClip engineLoopClip;

    [Header("Moving Object")]
    public Transform movingObject;
    public Transform objectDestination;
    public float moveSpeed = 1f;

    bool playerExited = false;
    bool returningFromOutside = false;

    protected override void Start()
    {
        returningFromOutside = Story.prologueOutsideDone;
        truckDoor.setCanEnterDoor(!returningFromOutside);
        truckDoor.onEnter.AddListener(() => playerExited = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!returningFromOutside)
        {
            // First time — player can leave freely
            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerExited);
            GoToScene(prologueOutsideSceneName);
        }
        else
        {
            // Returning from outside — sit, engine, move object, load next
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            PersistentServices.Instance.PlaySFX(engineStartClip);
            yield return new WaitForSeconds(engineStartClip.length);

            engineAudioSource.clip = engineLoopClip;
            engineAudioSource.loop = true;
            engineAudioSource.Play();

            yield return new WaitForSeconds(2f);
            StartCoroutine(MoveObjectAndLoadScene());
        }
    }

    IEnumerator MoveObjectAndLoadScene()
    {
        Vector3 start = movingObject.position;
        Vector3 end = objectDestination.position;
        float halfwayDistance = Vector3.Distance(start, end) / 2f;

        while (Vector3.Distance(movingObject.position, end) > halfwayDistance)
        {
            movingObject.position = Vector3.MoveTowards(
                movingObject.position,
                end,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        GoToScene(nextSceneName);
    }
}