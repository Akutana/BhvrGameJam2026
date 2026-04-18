using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDialogueManager : MonoBehaviour
{
    public AudioSource audioSource;

    private Queue<AudioClip> queue = new Queue<AudioClip>();

    private Coroutine playRoutine;


    public void PlayDialogue(AudioDialogue dialogue)
    {
        queue.Clear();

        foreach (var clip in dialogue.clips)
            queue.Enqueue(clip);

        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayQueue());
    }

    private IEnumerator PlayQueue()
    {
        while (queue.Count > 0)
        {
            AudioClip clip = queue.Dequeue();

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);

            // small random delay for radio feel
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }
}
