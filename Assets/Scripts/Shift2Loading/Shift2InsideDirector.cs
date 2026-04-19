using System.Collections;
using UnityEngine;

public class Shift2InsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public Transform sitPoint;
    public Transform exitPoint;
    public TapeInteractable tape;
    public Shift2FuelJerricanInteractable fuelJerrican;
    public string outsideSceneName;

    bool playerExited = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerExited = true);
        tape.gameObject.SetActive(false);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.shift2DrivingDialoguePlayed)
        {
            // ── First visit: arriving from loading scene ─────────────
            Story.shift2DrivingDialoguePlayed = true;
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;
            yield return PlayDialogue(0); // in-truck dialogue
            player.forcedToSit = false;
        }
        else if (!Story.shift2HasTape)
        {
            // ── Second visit: player came back to get tape ───────────
            tape.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => Story.shift2HasTape);
        }
        else
        {
            fuelJerrican.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => Story.shift2HasFuelJerrican);
        }

        truckDoor.setCanEnterDoor(true);
        yield return WaitUntilTrue(() => playerExited);

        GoToScene(outsideSceneName);
    }
}