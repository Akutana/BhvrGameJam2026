using System.Collections;
using UnityEngine;

public class Shift3LoadingDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public TargetZone targetZone;
    public string shift3InsideSceneName;

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

        // Wait for all 4 boxes to be loaded
        yield return WaitUntilTrue(() => targetZone.IsComplete());

        // Unlock door, no dialogue needed
        truckDoor.setCanEnterDoor(true);

        yield return WaitUntilTrue(() => playerEnteredTruck);

        GoToScene(shift3InsideSceneName);
    }
}