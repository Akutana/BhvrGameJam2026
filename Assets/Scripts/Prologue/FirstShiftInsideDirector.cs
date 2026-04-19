using System.Collections;
using UnityEngine;

public class FirstShiftInsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public string firstShiftOutsideSceneName;
    public string nextShiftSceneName;
    public AudioClip truckLoopClip;
    public Transform sitPoint;
    public Transform exitPoint;

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
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;
            yield return PlayDialogue(0);
            yield return PlayDialogue(1);
            yield return ShakeCamera();
            yield return PlayDialogue(2);
            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);

            yield return WaitUntilTrue(() => playerExited);
            Story.firstShiftInsideDone = true;
            GoToScene(firstShiftOutsideSceneName);
        }
        else if (Story.firstShiftOutsideDone)
        {
            // Returning after outside shift - sit player, play final dialogue, load next
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;
            yield return PlayDialogue(3); // final dialogue, assign in Inspector
            player.forcedToSit = false;
            GoToScene(nextShiftSceneName);
        }
        else
        {
            // Returning before outside is done - just let player move freely
            player.forcedToSit = false;
            truckDoor.setCanEnterDoor(true);

            yield return WaitUntilTrue(() => playerExited);
            Story.firstShiftInsideDone = true;
            GoToScene(firstShiftOutsideSceneName);
        }
    }
}