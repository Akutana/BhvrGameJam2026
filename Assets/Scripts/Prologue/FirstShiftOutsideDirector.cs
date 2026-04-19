using System.Collections;
using UnityEngine;

public class FirstShiftOutsideDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public TruckInvestigationTrigger[] investigationTriggers;
    public string firstShiftInsideSceneName;

    int triggersHit = 0;
    bool investigationComplete = false;
    bool playerEntered = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEntered = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.firstShiftOutsideIntroPlayed)
        {
            Story.firstShiftOutsideIntroPlayed = true;
            yield return PlayDialogue(0); // arrival dialogue
        }

        if (!Story.firstShiftTruckInspected)
        {
            yield return WaitUntilTrue(() => investigationComplete);
            yield return PlayDialogue(1); // investigation complete dialogue
            Story.firstShiftTruckInspected = true;
            truckDoor.setCanEnterDoor(true);
        }
        else
        {
            truckDoor.setCanEnterDoor(true);
        }

        yield return WaitUntilTrue(() => playerEntered);

        Story.firstShiftOutsideDone = true;
        GoToScene(firstShiftInsideSceneName);
    }

    public void OnInvestigationTriggerHit()
    {
        triggersHit++;
        if (triggersHit >= investigationTriggers.Length)
            investigationComplete = true;
    }
}