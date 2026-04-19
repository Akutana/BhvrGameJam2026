using System.Collections;
using UnityEngine;

public class PrologueDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public string firstShiftSceneName;
    public AudioClip truckLoopClip;
    public AudioClip truckStartClip;

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
            yield return PlayDialogue(0); // assign prologue dialogue in Inspector
        }

        yield return WaitUntilTrue(() => playerEnteredTruck);

        PersistentServices.Instance.PlaySFX(truckStartClip);
        Story.prologueDone = true;
        GoToScene(firstShiftSceneName);
    }
}