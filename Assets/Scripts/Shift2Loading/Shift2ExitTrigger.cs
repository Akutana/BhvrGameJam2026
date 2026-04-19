using UnityEngine;

public class Shift2ExitTrigger : MonoBehaviour
{
    public bool reached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            reached = true;
            gameObject.SetActive(false);
        }
    }
}