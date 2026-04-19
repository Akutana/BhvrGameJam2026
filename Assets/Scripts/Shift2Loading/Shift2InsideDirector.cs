using System.Collections;
using UnityEngine;

public class Shift2InsideDirector : SceneDirector
{
    public PlayerController player;
    public DoorInteractable truckDoor;
    public Transform sitPoint;
    public Transform exitPoint;

    [Header("Second Visit")]
    public TapeInteractable tape;

    [Header("Third Visit - Fuel Jerrican")]
    public Shift2FuelJerricanInteractable fuelJerrican;
    
    public string outsideSceneName;

    bool playerExited = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerExited = true);
        tape.gameObject.SetActive(false);
        fuelJerrican.gameObject.SetActive(false);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.shift2DrivingDialoguePlayed)
        {
            // ── First visit: arriving from loading ───────────────────
            Story.shift2DrivingDialoguePlayed = true;
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;
            yield return PlayDialogue(0);
            player.forcedToSit = false;

            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerExited);
            GoToScene(outsideSceneName);
        }
        else if (!Story.shift2HasTape)
        {
            // ── Second visit: pick up tape ───────────────────────────
            tape.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => Story.shift2HasTape);

            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerExited);
            GoToScene(outsideSceneName);
        }
        else if (!Story.shift2FuelTankRefilled)
        {
            // ── Third visit: pick up jerrican ────────────────────────────────
            fuelJerrican.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => Story.shift2HasFuelJerrican);

            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerExited);
            GoToScene(outsideSceneName);
        }
        else if (!Story.shift2FinalInsideDialoguePlayed)
        {
            // ── Fourth visit: fade in/out + dialogue + final target zone
            player.Sit(sitPoint, exitPoint);
            player.forcedToSit = true;
            Story.shift2FinalInsideDialoguePlayed = true;
            yield return PlayDialogue(2);
            player.forcedToSit = false;

            // Fade out → fade in to signal time passing
            GoToScene(outsideSceneName); // or a dedicated final scene
        }
    }
}