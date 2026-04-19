using UnityEngine;

public class AudioDialogueTrigger : MonoBehaviour
{
    public AudioDialogue dialogue;
    public AudioDialogueManager manager;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            manager.PlayDialogue(dialogue);
            hasPlayed = true;
        }
    }
}