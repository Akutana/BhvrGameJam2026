using System.Collections;
using UnityEngine;

public class Shift2LoadingDirector : SceneDirector
{
    public TargetZone targetZone;
    public DoorInteractable truckDoor;
    public string drivingSceneName;

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

        if (!Story.shift2IntroPlayed)
        {
            Story.shift2IntroPlayed = true;
            yield return PlayDialogue(0); // intro dialogue
        }

        yield return WaitUntilTrue(() => targetZone.IsComplete());
        Story.shift2TruckLoaded = true;

        truckDoor.setCanEnterDoor(true);
        yield return WaitUntilTrue(() => playerEnteredTruck);

        GoToScene(drivingSceneName);
    }
}