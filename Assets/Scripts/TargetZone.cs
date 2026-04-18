using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public Transform GetNextTargetPoint()
    {
        foreach (Transform targetPoint in transform)
        {
            if (targetPoint.childCount == 0)
                return targetPoint;
        }

        return null;
    }
}
