using UnityEngine;

public class Grabbable : Interactable
{
    public GameObject pickUpPos;
    private bool pickedUp = false;

    public override void Interact()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        pickedUp = !pickedUp;

        if (pickedUp)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.useGravity = true;
        }
    }
}
