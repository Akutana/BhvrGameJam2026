using UnityEngine;

public class Grabbable : Interactable
{
    public GameObject pickUpPos;
    private bool pickedUp = false;

    public override void Interact()
    {
        if (pickedUp)
        {
            transform.SetParent(null);

            transform.GetComponent<Rigidbody>().useGravity = true;

            pickedUp = false;
        }

        else
        {
            transform.SetParent(pickUpPos.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            transform.GetComponent<Rigidbody>().useGravity = false;

            pickedUp = true;
        }
    }
}
