using System.Collections;
using UnityEngine;

public class PrologueOutsideDirector : SceneDirector
{
    public static PrologueOutsideDirector Instance { get; private set; }

    public DoorInteractable truckDoor;
    public TargetZone targetZone;
    public string prologueInsideSceneName;

    bool playerEntered = false;

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

        // Arrival dialogue
        yield return PlayDialogue(0);

        // Wait for 4 boxes to be loaded
        yield return WaitUntilTrue(() => targetZone.IsComplete());

        // Post loading dialogue
        yield return PlayDialogue(1);

        // Unlock door
        truckDoor.setCanEnterDoor(true);

        yield return WaitUntilTrue(() => playerEntered);

        Story.prologueOutsideDone = true;
        GoToScene(prologueInsideSceneName);
    }
}