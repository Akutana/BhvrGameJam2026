using UnityEngine;

public class Shift2GenericInteractable : Interactable
{
    public bool isObjectA = true;

    public override void Interact()
    {
        if (isObjectA)
            GameManager.Instance.state.shift2ObjectADone = true;
        else
            GameManager.Instance.state.shift2ObjectBDone = true;

        gameObject.SetActive(false);
    }
}