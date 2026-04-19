using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioDialogueManager : MonoBehaviour
{
    public static AudioDialogueManager Instance;
    private AudioSource audioSource;
    private Queue<AudioClip> queue = new Queue<AudioClip>();
    private Coroutine routine;
    public Action OnDialogueFinished;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this from each SceneDirector's Start()
    public void SetAudioSource(AudioSource source)
    {
        audioSource = source;
    }

    public void PlayDialogue(AudioDialogue dialogue)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioDialogueManager has no AudioSource assigned.");
            return;
        }

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