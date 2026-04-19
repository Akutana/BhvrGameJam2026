using System.Collections;
using UnityEngine;

public class PrologueDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public string firstShiftSceneName;
    public AudioClip truckLoopClip;
    public AudioClip truckStartClip;
    public TargetZone targetZone;

    bool playerEnteredTruck = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEnteredTruck = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.prologueDialoguePlayed)
        {
            Story.prologueDialoguePlayed = true;
            yield return PlayDialogue(0); // first dialogue
        }

        // Wait for truck to be fully loaded
        yield return WaitUntilTrue(() => targetZone.IsComplete());

        // Play dialogue after loading is done
        yield return PlayDialogue(1); // loading complete dialogue

        // Unlock door after dialogue finishes
        truckDoor.setCanEnterDoor(true);

        yield return WaitUntilTrue(() => playerEnteredTruck);

        PersistentServices.Instance.PlaySFX(truckStartClip);
        Story.prologueDone = true;
        GoToScene(firstShiftSceneName);
    }
}