using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalOutsideDirector : SceneDirector
{
    public DoorInteractable truckDoor;

    [Header("Engine Interactable")]
    public Interactable engineInteractable;

    [Header("Audio")]
    public AudioSource engineAudioSource;
    public AudioClip engineStartClip;
    public AudioClip doorLockClip;
    public AudioClip doorHandleClip;

    [Header("Lights")]
    public Light[] lights;

    [Header("Ending Model")]
    public Transform endingModel;
    public Transform endingModelDestination;
    public float endingModelSpeed = 0.5f;

    [Header("Moving Object")]
    public Transform movingObject;
    public Transform objectDestination;
    public float moveSpeed = 1f;

    [Header("Dialogue Locations")]
    public AudioSource secondDialogueSource;

    bool engineFixed = false;
    bool playerEnteredTruck = false;
    bool objectReached = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEnteredTruck = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        yield return WaitUntilTrue(() => engineFixed);

        PersistentServices.Instance.PlaySFX(engineStartClip);

        foreach (Light l in lights)
        {
            l.enabled = true;
            LightFlicker flicker = l.GetComponent<LightFlicker>();
            if (flicker != null) flicker.enabled = false;
        }

        truckDoor.setCanEnterDoor(true);

        yield return WaitUntilTrue(() => playerEnteredTruck);

        PersistentServices.Instance.PlaySFX(doorLockClip);
        yield return new WaitForSeconds(doorLockClip.length);
        PersistentServices.Instance.PlaySFX(doorHandleClip);
        yield return new WaitForSeconds(doorHandleClip.length);

        StartCoroutine(MoveEndingModel());

        yield return PlayDialogue(0);

        yield return PlayDialogueAtSource(1, secondDialogueSource);

        StartCoroutine(MoveObject());
        yield return PlayDialogue(2);

        yield return new WaitForSeconds(10f);
        yield return FadeToBlack();
    }

    IEnumerator MoveEndingModel()
    {
        endingModel.gameObject.SetActive(true);
        while (Vector3.Distance(endingModel.position, endingModelDestination.position) > 0.01f)
        {
            endingModel.position = Vector3.MoveTowards(
                endingModel.position,
                endingModelDestination.position,
                endingModelSpeed * Time.deltaTime
            );
            yield return null;
        }
        endingModel.position = endingModelDestination.position;
    }

    IEnumerator PlayDialogueAtSource(int index, AudioSource source)
    {
        AudioDialogueManager.Instance.SetAudioSource(source);
        yield return PlayDialogue(index);
        AudioDialogueManager.Instance.SetAudioSource(playerVoiceSource);
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

    IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        float duration = PersistentServices.Instance.fadeDuration;
        RawImage fadeImage = PersistentServices.Instance.fadeImage;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, elapsed / duration));
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1f);
    }

    public void OnEngineFixed()
    {
        engineFixed = true;
    }
}