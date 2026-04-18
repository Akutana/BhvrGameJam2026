using UnityEngine;

public class Prologue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip truckSound;
    public string nextSceneName;
    public DoorInteractable doorInteractable;

    bool isFinishedLoading = false;
    bool audioStarted = false;
    bool isLoadingScene = false;
    bool enteredTruck = false;

    void Start()
    {
        doorInteractable.onEnter.AddListener(OnPlayerEnteredTruck);
    }

    void Update()
    {
        if (isFinishedLoading && !audioStarted)
        {
            audioSource.Play();
            audioStarted = true;
        }

        if (audioStarted && !audioSource.isPlaying)
            doorInteractable.setCanEnterDoor(true);

        if (enteredTruck && !isLoadingScene)
        {
            isLoadingScene = true;
            audioSource.PlayOneShot(truckSound);
            SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
        }
    }

    void OnPlayerEnteredTruck()
    {
        enteredTruck = true;
    }
}