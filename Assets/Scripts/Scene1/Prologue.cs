using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip truckSound;
    public string nextSceneName;

    bool isFinishedLoading = false;
    bool audioStarted = false;
    bool canEnterTruck = false;
    bool isLoadingScene = false;

    public float timeLeft = 60.0f;

    void Update()
    {
        if (isFinishedLoading && !audioStarted)
        {
            audioSource.Play();
            audioStarted = true;
        }

        if (audioStarted && !audioSource.isPlaying)
            canEnterTruck = true;

        if (canEnterTruck && !isLoadingScene)
        {
            if (true) // replace with door interaction condition
            {
                isLoadingScene = true;
                audioSource.PlayOneShot(truckSound);
                SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
            }
        }
    }
}