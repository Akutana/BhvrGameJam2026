using UnityEngine;

public class Grabbable : Interactable
{
    public GameObject pickUpPos;
    private bool pickedUp = false; 
    private bool canBeGrabbed = true; 
    private bool isPlaced = false;

    public override void Interact()
    {
        if (!pickedUp && !canBeGrabbed)
            return;

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

    private void OnTriggerStay(Collider other)
    {
        if (!pickedUp && other.CompareTag("TruckTarget") && (canBeGrabbed == true))
        {
            canBeGrabbed = false;
            isPlaced = true;

            Transform target = other.GetComponent<TargetZone>().GetNextTargetPoint();

            if (target != null)
            {
                transform.SetParent(target, true);
                transform.position = target.position;
                transform.rotation = target.rotation;
            }


            else return;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TruckTarget") && !isPlaced)
        {
            Debug.Log("fhdsjakdlhsjaklfhsdjkla");
            canBeGrabbed = true;
        }
    }

    public bool CanBeGrabbed()
    {
        return canBeGrabbed;
    }
}
