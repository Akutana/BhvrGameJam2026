using UnityEngine;

public class Shift2MovingSoundTrigger : MonoBehaviour
{
    public AudioClip movingSound;
    public GameObject attachedObject;  // the object the sound is attached to
    public bool isFirstTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PersistentServices.Instance.PlaySFX(movingSound);

        if (attachedObject != null)
            attachedObject.SetActive(true); // or move it, animate it, etc.

        if (isFirstTrigger)
            GameManager.Instance.state.shift2MovingSound1Done = true;
        else
            GameManager.Instance.state.shift2MovingSound2Done = true;

        gameObject.SetActive(false);
    }
}