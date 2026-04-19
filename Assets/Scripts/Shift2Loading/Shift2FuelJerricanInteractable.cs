using Unity.VisualScripting;
using UnityEngine;

public class Shift2FuelJerricanInteractable : Interactable
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Interact()
    {
        GameManager.Instance.state.shift2HasFuelJerrican = true;
        gameObject.SetActive(false);
    }
}
