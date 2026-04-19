using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirstDelivery : MonoBehaviour
{
    public static FirstDelivery Instance { get; private set; }

    [Header("Audio")]
    public AudioSource truckAudioSource;
    public AudioClip truckLoop;
    public AudioDialogue firstDialogue;
    public AudioDialogue secondDialogue;

    [Header("Camera Shake")]
    public Transform cameraTransform;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    [Header("References")]
    public PlayerController player;
    public string outsideSceneName;

    // Persistent flags
    static bool hasPlayedIntroSequence = false;
    public static bool hasInspectedTruck = false;
    public static bool canLeave = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        truckAudioSource.clip = truckLoop;
        truckAudioSource.loop = true;
        truckAudioSource.Play();

        SceneTransitionManager.Instance.FadeIn();

        if (!hasPlayedIntroSequence)
        {
            hasPlayedIntroSequence = true;
            player.forcedToSit = true;
            StartCoroutine(IntroSequence());
        }
        else
        {
            player.forcedToSit = false;
        }
    }

    // Hook this to DoorInteractable onEnter event in Inspector
    public void GoOutside()
    {
        SceneTransitionManager.Instance.TransitionToScene(outsideSceneName);
    }

    public void OnTruckInspected()
    {
        hasInspectedTruck = true;
        canLeave = true;
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(SceneTransitionManager.Instance.fadeDuration);
        yield return StartCoroutine(PlayDialogueAndWait(firstDialogue));
        yield return StartCoroutine(ShakeCamera());
        yield return StartCoroutine(PlayDialogueAndWait(secondDialogue));
        player.forcedToSit = false;
    }

    IEnumerator PlayDialogueAndWait(AudioDialogue dialogue)
    {
        bool finished = false;
        AudioDialogueManager.Instance.OnDialogueFinished += () => finished = true;
        AudioDialogueManager.Instance.PlayDialogue(dialogue);
        yield return new WaitUntil(() => finished);
        AudioDialogueManager.Instance.OnDialogueFinished -= () => finished = true;
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            cameraTransform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            timer += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}