using System.Collections;
using UnityEngine;

public class Base3OutsideDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public TargetZone targetZone;
    public string base3InsideSceneName;

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

        // Wait for 2 boxes to be loaded
        yield return WaitUntilTrue(() => targetZone.IsComplete());

        // TODO: call function here

        // Dialogue after loading
        yield return PlayDialogue(0);

        // Unlock door
        truckDoor.setCanEnterDoor(true);

        // Wait for player to enter truck
        yield return WaitUntilTrue(() => playerEnteredTruck);

        Story.base3OutsideDone = true;
        GoToScene(base3InsideSceneName);
    }
}