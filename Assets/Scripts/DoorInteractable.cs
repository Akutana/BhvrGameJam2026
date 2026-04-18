using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : Interactable
{
    public bool canEnterDoor = false;
    public UnityEvent onEnter;

    public override void Interact()
    {
        if (!canEnterDoor) return;
        onEnter.Invoke();
        Debug.Log("Interaction with " + gameObject.name);
    }

    public void setCanEnterDoor(bool value)
    {
        canEnterDoor = value;
        interactionText = "Enter truck";
    }
}