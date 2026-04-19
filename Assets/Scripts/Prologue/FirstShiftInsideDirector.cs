using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirstShiftInsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string firstShiftOutsideSceneName;
    public string nextShiftSceneName;

    public Transform sitPoint;
    public Transform exitPoint;

    [Header("Engine Audio")]
    public AudioSource engineAudioSource;
    public AudioClip engineLoopClip;
    public AudioClip shakeNoiseClip;

    [Header("Moving Object - First Pass")]
    public Transform movingObjectA;
    public Transform destinationA;
    public float moveSpeedA = 1f;

    [Header("Moving Object - Second Pass")]
    public Transform movingObjectB;
    public Transform destinationB;
    public float moveSpeedB = 1f;

    bool playerExited = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerExited = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.firstShiftIntroPlayed)
        {
            Story.firstShiftIntroPlayed = true;

            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            // Engine loop starts immediately
            engineAudioSource.clip = engineLoopClip;
            engineAudioSource.loop = true;
            engineAudioSource.Play();

            // First dialogue
            yield return PlayDialogue(0);

            // Quick fade out and in
            yield return FadeTo(1f);
            yield return FadeTo(0f);

            // Dialogue after fade
            yield return PlayDialogue(1);

            // Wait 15 seconds
            yield return new WaitForSeconds(15f);

            // Start moving object A, play dialogue at the same time
            StartCoroutine(MoveObject(movingObjectA, destinationA, moveSpeedA));
            yield return PlayDialogue(2);

            // Shake camera and play noise at the same time
            StartCoroutine(ShakeCamera());
            PersistentServices.Instance.PlaySFX(shakeNoiseClip);
            yield return new WaitForSeconds(shakeDuration);

            // Dialogue after shake
            yield return PlayDialogue(3);

            // TODO: call function here

            // More dialogue
            yield return PlayDialogue(4);

            // Unforce sit, unlock door
            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);

            yield return WaitUntilTrue(() => playerExited);
            Story.firstShiftInsideDone = true;
            GoToScene(firstShiftOutsideSceneName);
        }
        else if (Story.firstShiftOutsideDone)
        {
            // Returning from outside — force sit
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            // Start moving object B
            StartCoroutine(MoveObjectWithFadeAndDialogue());
        }
        else
        {
            // Returning before outside done
            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerExited);
            Story.firstShiftInsideDone = true;
            GoToScene(firstShiftOutsideSceneName);
        }
    }

    IEnumerator MoveObjectWithFadeAndDialogue()
    {
        // Start moving object
        StartCoroutine(MoveObject(movingObjectB, destinationB, moveSpeedB));

        // Halfway through movement, fade out and in
        float totalDistance = Vector3.Distance(movingObjectB.position, destinationB.position);
        yield return new WaitUntil(() =>
            Vector3.Distance(movingObjectB.position, destinationB.position) <= totalDistance / 2f);

        yield return FadeTo(1f);
        yield return FadeTo(0f);

        // Object still moving, play dialogue
        yield return PlayDialogue(5);

        GoToScene(nextShiftSceneName);
    }

    IEnumerator MoveObject(Transform obj, Transform destination, float speed)
    {
        while (Vector3.Distance(obj.position, destination.position) > 0.01f)
        {
            obj.position = Vector3.MoveTowards(
                obj.position,
                destination.position,
                speed * Time.deltaTime
            );
            yield return null;
        }
        obj.position = destination.position;
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float elapsed = 0f;
        float duration = PersistentServices.Instance.fadeDuration;
        RawImage fadeImage = PersistentServices.Instance.fadeImage;
        float startAlpha = fadeImage.color.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}