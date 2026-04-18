using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float range = 10f;

    private Interactable currentInteractable;
    public KeyCode interactKey = KeyCode.E;

    public TextMeshProUGUI interactionTextUI;

    private Interactable heldObject;
    public float holdDistance = 2f;
    public float moveSpeed = 10f;

    void Update()
    {
        if (heldObject != null)
        {
            Transform obj = heldObject.transform;

            Vector3 targetPos = cam.transform.position + cam.transform.forward * holdDistance;

            // mouvement fluide
            obj.position = Vector3.Lerp(obj.position, targetPos, Time.deltaTime * moveSpeed);

            // rotation face caméra
            obj.rotation = Quaternion.Lerp(obj.rotation, cam.transform.rotation, Time.deltaTime * moveSpeed);

            if (Input.GetKeyDown(interactKey))
            {
                heldObject.Interact();
                heldObject = null;
            }

            interactionTextUI.text = $"Drop [{interactKey}]";
            if (!interactionTextUI.gameObject.activeSelf)
                interactionTextUI.gameObject.SetActive(true);

            return;
        }

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
                {
                    interactable.Interact();

                    if (!heldObject && interactable is Grabbable)
                        heldObject = interactable;
                }

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