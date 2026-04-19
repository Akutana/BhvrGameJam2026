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

    public static FirstShiftOutsideDirector Instance { get; private set; }

    protected override void Start()
    {
        Instance = this;
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEntered = true);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.firstShiftTruckInspected)
        {
            yield return WaitUntilTrue(() => investigationComplete);
            yield return PlayDialogue(0); // investigation complete dialogue
            Story.firstShiftTruckInspected = true;
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