using UnityEngine;

public class TargetZone : MonoBehaviour
{

    public DoorInteractable doorInteractable;

    public Transform GetNextTargetPoint()
    {
        foreach (Transform targetPoint in transform)
        {
            if (targetPoint.childCount == 0)
            {
                return targetPoint;
            }
                
        }

        return null;
    }

    public void setCanEnterDoor()
    {
        if (doorInteractable != null)
        {
            doorInteractable.setCanEnterDoor(true);
        }
    }
}
