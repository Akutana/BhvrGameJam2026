using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionText = "Interact";

    public virtual void Interact()
    {
        Debug.Log("Interaction with " + gameObject.name);
    }

    public virtual bool CanBeInteractedWith()
    {
        return true;
    }
}