using UnityEngine;

public class Interactable : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public string interactionText = "Press E";

    public virtual void Interact()
    {
        Debug.Log("Interaction with " + gameObject.name);
    }
}