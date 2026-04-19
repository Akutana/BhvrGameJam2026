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
                return targetPoint;
        }
        return null;
    }

    public void CheckCompletion()
    {
        if (IsComplete())
            doorInteractable.setCanEnterDoor(true);
    }

    public bool IsComplete()
    {
        int filledCount = 0;
        foreach (Transform targetPoint in transform)
        {
            if (targetPoint.childCount > 0)
                filledCount++;
        }
        return filledCount >= requiredItems;
    }
}