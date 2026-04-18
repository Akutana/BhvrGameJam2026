using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float range = 10f;

    private Interactable currentInteractable;
    public KeyCode interactKey = KeyCode.E;

    public TextMeshProUGUI interactionTextUI;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Interactable interactable = hit.transform.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;

                    string text = GetInteractionText(interactable);
                    interactionTextUI.text = text; 
                    
                    if (!interactionTextUI.gameObject.activeSelf)
                        interactionTextUI.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(interactKey))
                    interactable.Interact();

                return;
            }
        }

        currentInteractable = null;
        interactionTextUI.gameObject.SetActive(false);
    }

    string GetInteractionText(Interactable interactable)
    {
        return $"{interactable.interactionText} [{interactKey}]";
    }
}