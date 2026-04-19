using UnityEngine;
using UnityEngine.Events;

public class ChairInteractable : Interactable
{
    public Transform sitPoint;
    public Transform exitPoint;
    public PlayerController playerController;
    public UnityEvent onSit;

    void Start()
    {
        interactionText = "Sit";
    }

    public override void Interact()
    {
        playerController.Sit(sitPoint, exitPoint);
        onSit.Invoke();
    }
}