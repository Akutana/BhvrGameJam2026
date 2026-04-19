using System.Collections;
using UnityEngine;

public class Shift2CabDirector : SceneDirector
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

        player.Sit(sitPoint, exitPoint);
        player.forcedToSit = true;
        yield return PlayDialogue(0); // tape pickup dialogue
        Story.shift2HasTape = true;
        player.forcedToSit = false;

        truckDoor.setCanEnterDoor(true);
        yield return WaitUntilTrue(() => playerExited);

        GoToScene(outsideSceneName);
    }
}