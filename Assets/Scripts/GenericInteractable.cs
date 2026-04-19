using UnityEngine;
using UnityEngine.Events;

public class GenericInteractable : Interactable
{
    public UnityEvent onInteract;
    public bool oneShot = true; // if true, can only be interacted with once
    private bool hasInteracted = false;

    public override void Interact()
    {
        if (oneShot && hasInteracted) return;
        hasInteracted = true;
        onInteract.Invoke();
        Debug.Log("Interaction with " + gameObject.name);
    }

    public override bool CanBeInteractedWith()
    {
        if (oneShot && hasInteracted) return false;
        return true;
    }

    public void Reset()
    {
        hasInteracted = false;
    }
}