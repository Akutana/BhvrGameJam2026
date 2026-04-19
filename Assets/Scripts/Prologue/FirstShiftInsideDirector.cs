using System.Collections;
using UnityEngine;

public class FirstShiftInsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string firstShiftOutsideSceneName;
    public AudioClip truckLoopClip;

    bool playerExited = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerExited = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.firstShiftIntroPlayed)
        {
            Story.firstShiftIntroPlayed = true;
            player.forcedToSit = true;
            yield return PlayDialogue(0); // first dialogue
            yield return ShakeCamera();
            yield return PlayDialogue(1); // second dialogue
            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);
        }
        else
        {
            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);
        }

        yield return WaitUntilTrue(() => playerExited);

        Story.firstShiftInsideDone = true;
        GoToScene(firstShiftOutsideSceneName);
    }
}