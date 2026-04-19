using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public DoorInteractable doorInteractable;
    public int requiredItems = 4;

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
            Debug.Log("not null");
            doorInteractable.setCanEnterDoor(true);
        }

        else Debug.Log("null");
    }

    public void CheckCompletion()
    {
        int filledCount = 0;
        foreach (Transform targetPoint in transform)
        {
            if (targetPoint.childCount > 0)
                filledCount++;
        }

        Debug.Log($"Filled: {filledCount} / {requiredItems}");

        if (filledCount >= requiredItems)
            doorInteractable.setCanEnterDoor(true);
    }
}
