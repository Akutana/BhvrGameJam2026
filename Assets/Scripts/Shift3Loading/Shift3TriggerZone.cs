using UnityEngine;

public class Shift3TriggerZone : MonoBehaviour
{
    bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        Shift3OutsideDirector.Instance.OnPlayerReachedZone();
    }
}