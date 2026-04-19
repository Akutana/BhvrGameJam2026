using UnityEngine;

public class TapeInteractable : Interactable
{
    void Start()
    {
        interactionText = "Pick up tape";
    }

    public override void Interact()
    {
        GameManager.Instance.state.shift2HasTape = true;
        gameObject.SetActive(false);
    }
}