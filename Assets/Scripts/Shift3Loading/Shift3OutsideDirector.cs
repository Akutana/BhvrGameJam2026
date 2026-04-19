using System.Collections;
using UnityEngine;

public class Shift3OutsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string insideSceneName;

    public static Shift3OutsideDirector Instance { get; private set; }

    bool reachedTriggerZone = false;
    bool playerEnteredTruck = false;

    protected override void Start()
    {
        Instance = this;
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEnteredTruck = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        // Wait for player to reach trigger zone
        yield return WaitUntilTrue(() => reachedTriggerZone);

        // Dialogue at trigger zone
        yield return PlayDialogue(0);

        // Unlock truck door to go back inside
        truckDoor.setCanEnterDoor(true);

        // Wait for player to enter truck
        yield return WaitUntilTrue(() => playerEnteredTruck);

        GoToScene(insideSceneName);
    }

    public void OnPlayerReachedZone()
    {
        reachedTriggerZone = true;
    }
}