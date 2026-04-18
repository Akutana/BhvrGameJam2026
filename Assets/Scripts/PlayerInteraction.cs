using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float range = 10f;

    private Interactable currentInteractable;

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
                    Debug.Log(interactable.interactionText);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }

                return;
            }
        }

        currentInteractable = null;
    }
}