using UnityEngine;

public class TruckInvestigationTrigger : MonoBehaviour
{
    bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Something entered trigger: {other.name} tag: {other.tag}");
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        gameObject.SetActive(false);
        Debug.Log($"FirstShiftOutsideDirector.Instance null: {FirstShiftOutsideDirector.Instance == null}");
        FirstShiftOutsideDirector.Instance.OnInvestigationTriggerHit();
    }
}