using System.Collections;
using UnityEngine;

public class Shift2OutsideDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public Shift2ExitTrigger exitTrigger;

    [Header("Fuel Tank")]
    public FuelTankInteractable fuelTank;
    public AudioClip fuelSFX;
    public GameObject objectToMove;
    public Transform objectMoveTarget;

    [Header("Final Outside")]
    public Shift2EndTrigger endTrigger;
    public Shift2GenericInteractable objectA;
    public Shift2GenericInteractable objectB; 
    
    [Header("Final Target Zone")]
    public TargetZone finalTargetZone;

    [Header("Third Visit - Moving Sounds")]
    public Shift2MovingSoundTrigger movingSoundTrigger1;
    public Shift2MovingSoundTrigger movingSoundTrigger2;

    public string insideSceneName;
    public string nextShiftSceneName;

    bool playerEnteredTruck = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEnteredTruck = true);
        fuelTank.gameObject.SetActive(false);
        endTrigger.gameObject.SetActive(false);
        objectA.gameObject.SetActive(false);
        objectB.gameObject.SetActive(false);
        movingSoundTrigger1.gameObject.SetActive(false);
        movingSoundTrigger2.gameObject.SetActive(false);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.shift2HasTape)
        {
            // ── First visit: reach exit trigger → dialogue → enter truck ─
            yield return WaitUntilTrue(() => exitTrigger.reached);

            if (!Story.shift2OutsideDialoguePlayed)
            {
                Story.shift2OutsideDialoguePlayed = true;
                yield return PlayDialogue(0);
            }

            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerEnteredTruck);
            GoToScene(insideSceneName);
        }
        else if (!Story.shift2FuelTankDone)
        {
            // ── Second visit: tape in hand → interact fuel tank ──────────
            exitTrigger.gameObject.SetActive(false);
            fuelTank.gameObject.SetActive(true);

            yield return WaitUntilTrue(() => Story.shift2FuelTankDone);

            PersistentServices.Instance.PlaySFX(fuelSFX);
            if (objectToMove != null && objectMoveTarget != null)
            {
                objectToMove.transform.position = objectMoveTarget.position;
                objectToMove.transform.rotation = objectMoveTarget.rotation;
            }

            if (!Story.shift2FuelDialoguePlayed)
            {
                Story.shift2FuelDialoguePlayed = true;
                yield return PlayDialogue(1);
            }

            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerEnteredTruck);
            GoToScene(insideSceneName);
        }
        else if (!Story.shift2FuelTankRefilled)
        {
            // ── Third visit: jerrican in hand → refill fuel tank → moving sounds → enter truck ─
            exitTrigger.gameObject.SetActive(false);
            fuelTank.gameObject.SetActive(true);

            yield return WaitUntilTrue(() => Story.shift2FuelTankRefilled);

            movingSoundTrigger1.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => Story.shift2MovingSound1Done);

            movingSoundTrigger2.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => Story.shift2MovingSound2Done);

            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerEnteredTruck);
            GoToScene(insideSceneName);
        }
        else if (!Story.shift2FinalInsideDialoguePlayed)
        {
            // ── Fourth visit: waiting for inside scene to handle dialogue/fade ─
            // Nothing to do outside, director just sent player to inside scene
            // This branch should never actually run; GoToScene fires immediately
        }
        else
        {
            // ── Fifth visit: final target zone ───────────────────────────────
            exitTrigger.gameObject.SetActive(false);

            yield return PlayDialogue(3);

            yield return WaitUntilTrue(() => finalTargetZone.IsComplete());
            Story.shift2FinalTruckLoaded = true;

            GoToScene(nextShiftSceneName);
        }
    }
}