using System.Collections;
using UnityEngine;

public class OutsideTruck : MonoBehaviour
{
    public static OutsideTruck Instance { get; private set; }

    [Header("Dialogue")]
    public AudioDialogue arrivalDialogue;
    public AudioDialogue investigationDialogue;

    [Header("References")]
    public TruckInvestigationTrigger[] investigationTriggers; // 4 triggers
    public DoorInteractable truckDoor; // the door back to first delivery scene
    public string firstDeliverySceneName;

    static bool hasPlayedArrivalDialogue = false;
    int triggersHit = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        truckDoor.setCanEnterDoor(false); // door locked until investigation done

        SceneTransitionManager.Instance.FadeIn();

        if (!hasPlayedArrivalDialogue)
        {
            hasPlayedArrivalDialogue = true;
            StartCoroutine(ArrivalSequence());
        }
    }

    IEnumerator ArrivalSequence()
    {
        yield return new WaitForSeconds(SceneTransitionManager.Instance.fadeDuration);
        yield return StartCoroutine(PlayDialogueAndWait(arrivalDialogue));
    }

    public void OnTriggerHit()
    {
        triggersHit++;
        if (triggersHit >= 4)
            StartCoroutine(InvestigationCompleteSequence());
    }

    IEnumerator InvestigationCompleteSequence()
    {
        yield return StartCoroutine(PlayDialogueAndWait(investigationDialogue));
        truckDoor.setCanEnterDoor(true); // unlock door
    }

    IEnumerator PlayDialogueAndWait(AudioDialogue dialogue)
    {
        bool finished = false;
        AudioDialogueManager.Instance.OnDialogueFinished += () => finished = true;
        AudioDialogueManager.Instance.PlayDialogue(dialogue);
        yield return new WaitUntil(() => finished);
        AudioDialogueManager.Instance.OnDialogueFinished -= () => finished = true;
    }

    public void ReturnToTruck()
    {
        FirstDelivery.hasInspectedTruck = true;
        SceneTransitionManager.Instance.TransitionToScene(firstDeliverySceneName);
    }
}