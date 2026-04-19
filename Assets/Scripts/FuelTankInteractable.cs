using UnityEngine;

public class FuelTankInteractable : Interactable
{
    void Start()
    {
        interactionText = "Apply tape to fuel tank";
    }

    public override bool CanBeInteractedWith()
    {
        return (GameManager.Instance.state.shift2HasTape
            && !GameManager.Instance.state.shift2FuelTankDone)
            || GameManager.Instance.state.shift2HasFuelJerrican;
    }

    public override void Interact()
    {
        if (!GameManager.Instance.state.shift2HasFuelJerrican)
        {
            if (!CanBeInteractedWith()) return;
            GameManager.Instance.state.shift2FuelTankDone = true;
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.state.shift2FuelTankRefilled = true;
        }
    }
}