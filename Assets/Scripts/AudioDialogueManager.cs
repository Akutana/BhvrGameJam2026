using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioDialogueManager : MonoBehaviour
{
    public static AudioDialogueManager Instance;

    public AudioSource audioSource;

    private Queue<AudioClip> queue = new Queue<AudioClip>();
    private Coroutine routine;

    public Action OnDialogueFinished; 

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    public void PlayDialogue(AudioDialogue dialogue)
    {
        queue.Clear();

        foreach (var clip in dialogue.clips)
            queue.Enqueue(clip);

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(PlayQueue());
    }

    private IEnumerator PlayQueue()
    {
        while (queue.Count > 0)
        {
            var clip = queue.Dequeue();

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
        OnDialogueFinished?.Invoke();
    }
}