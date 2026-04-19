using System.Collections;
using UnityEngine;

public class Shift3InsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string shift3OutsideSceneName;
    public string nextSceneName;

    public Transform sitPoint;
    public Transform exitPoint;

    public AudioClip truckStartClip;

    public GameObject objectToMove;
    public Transform objectTargetPosition;

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

        if (!Story.shift3InsideDone)
        {
            // First time entering � intro sequence
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            yield return PlayDialogue(0);

            PersistentServices.Instance.PlaySFX(truckStartClip);

            yield return PlayDialogue(1);

            if (objectToMove != null && objectTargetPosition != null)
                objectToMove.transform.position = objectTargetPosition.position;

            yield return StartCoroutine(FadeOutAndIn());

            yield return PlayDialogue(2);

            // TODO: call function here

            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);

            yield return WaitUntilTrue(() => playerExited);

            Story.shift3InsideDone = true;
            GoToScene(shift3OutsideSceneName);
        }
        else
        {
            // Returning from outside � force sit, call function, final dialogue, load next
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;

            // TODO: call function here

            yield return PlayDialogue(3); // final dialogue after returning from outside

            GoToScene(nextSceneName); // assign in Inspector
        }
    }

    IEnumerator FadeOutAndIn()
    {
        yield return StartCoroutine(FadeTo(1f));
        yield return StartCoroutine(FadeTo(0f));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float elapsed = 0f;
        float startAlpha = PersistentServices.Instance.fadeImage.color.a;
        float duration = PersistentServices.Instance.fadeDuration;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            PersistentServices.Instance.fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        PersistentServices.Instance.fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}