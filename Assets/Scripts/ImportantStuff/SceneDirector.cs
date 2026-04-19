using System.Collections;
using UnityEngine;

public abstract class SceneDirector : MonoBehaviour
{
    [Header("Dialogues")]
    public AudioDialogue[] dialogues;

    [Header("Player")]
    public AudioSource playerVoiceSource;

    [Header("Camera Shake")]
    public Transform cameraTransform;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    protected StoryState Story => GameManager.Instance.state;
    protected float FadeDuration => PersistentServices.Instance.fadeDuration;

    protected virtual void Start()
    {
        AudioDialogueManager.Instance.SetAudioSource(playerVoiceSource);
        StartCoroutine(DirectScene());
    }

    protected abstract IEnumerator DirectScene();

    protected IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(FadeDuration);
    }

    protected IEnumerator PlayDialogue(int index)
    {
        if (index >= dialogues.Length) yield break;
        bool finished = false;
        AudioDialogueManager.Instance.OnDialogueFinished += () => finished = true;
        AudioDialogueManager.Instance.PlayDialogue(dialogues[index]);
        yield return new WaitUntil(() => finished);
        AudioDialogueManager.Instance.OnDialogueFinished -= () => finished = true;
    }

    protected IEnumerator ShakeCamera()
    {
        if (cameraTransform == null) yield break;
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

    protected IEnumerator WaitUntilTrue(System.Func<bool> condition)
    {
        yield return new WaitUntil(condition);
    }

    protected void GoToScene(string sceneName)
    {
        PersistentServices.Instance.LoadScene(sceneName);
    }
}