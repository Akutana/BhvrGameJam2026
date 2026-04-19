using System.Collections;
using UnityEngine;

public class Shift2OutsideDirector : SceneDirector
{
    public DoorInteractable truckDoor;
    public Shift2ExitTrigger exitTrigger;
    public FuelTankInteractable fuelTank;
    public AudioClip fuelSFX;
    public GameObject objectToMove;
    public Transform objectMoveTarget;
    public Shift2EndTrigger endTrigger;
    public AudioSource playerWalkSource;
    public float walkSoundDoubleDelay = 0.5f;
    public string insideSceneName;
    public string nextShiftSceneName;

    bool playerEnteredTruck = false;

    protected override void Start()
    {
        truckDoor.setCanEnterDoor(false);
        truckDoor.onEnter.AddListener(() => playerEnteredTruck = true);
        fuelTank.gameObject.SetActive(false);
        endTrigger.gameObject.SetActive(false);
        base.Start();
    }

    protected override IEnumerator DirectScene()
    {
        yield return WaitForFade();

        if (!Story.shift2HasTape)
        {
            // ── First visit ──────────────────────────────────────────

            // Wait for player to reach the exit trigger zone
            yield return WaitUntilTrue(() => exitTrigger.reached);

            if (!Story.shift2OutsideDialoguePlayed)
            {
                Story.shift2OutsideDialoguePlayed = true;
                yield return PlayDialogue(0); // outside dialogue
            }

            // Send player to cab to get tape
            truckDoor.setCanEnterDoor(true);
            yield return WaitUntilTrue(() => playerEnteredTruck);

            GoToScene(insideSceneName);
        }
        else
        {
            // ── Second visit (returning with tape) ───────────────────

            fuelTank.gameObject.SetActive(true);

            yield return WaitUntilTrue(() => Story.shift2FuelTankDone);

            PersistentServices.Instance.PlaySFX(fuelSFX);

            if (!Story.shift2FuelDialoguePlayed)
            {
                Story.shift2FuelDialoguePlayed = true;
                yield return PlayDialogue(1); // post-fuel dialogue
            }

            if (objectToMove != null && objectMoveTarget != null)
            {
                objectToMove.transform.position = objectMoveTarget.position;
                objectToMove.transform.rotation = objectMoveTarget.rotation;
            }

            endTrigger.gameObject.SetActive(true);
            yield return WaitUntilTrue(() => endTrigger.reached);

            yield return new WaitForSeconds(walkSoundDoubleDelay);
            if (playerWalkSource != null)
                playerWalkSource.volume *= 2f;

            Story.shift2Done = true;
            GoToScene(nextShiftSceneName);
        }
    }
}