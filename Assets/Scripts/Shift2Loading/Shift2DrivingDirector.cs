using System.Collections;
using UnityEngine;

public class Shift2DrivingDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public Transform sitPoint;
    public Transform exitPoint;
    public string outsideSceneName;

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

        if (!Story.shift2DrivingDialoguePlayed)
        {
            Story.shift2DrivingDialoguePlayed = true;
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;
            yield return PlayDialogue(0); // in-truck dialogue
            player.forcedToSit = false;
        }

        truckDoor.setCanEnterDoor(true);
        yield return WaitUntilTrue(() => playerExited);

        GoToScene(outsideSceneName);
    }
}