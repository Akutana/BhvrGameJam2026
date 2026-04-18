using UnityEngine;

public class TruckInvestigationTrigger : MonoBehaviour
{
    bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        gameObject.SetActive(false); // hide trigger after hit
        OutsideTruck.Instance.OnTriggerHit();
    }
}