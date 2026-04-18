using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float climbingSpeed = 2f;

    [Header("Crouching")]
    public float crouchSpeed = 2.5f;
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchTransitionSpeed = 8f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isCrouching = false;
    private float standCameraY;
    private float crouchCameraY;
    private bool isOnLadder = false;

    public enum State { Walking, Climbing }
    public State currentState = State.Walking;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        standHeight = controller.height;
        standCameraY = controller.height - 0.2f;
        crouchCameraY = crouchHeight - 0.2f;

        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = standCameraY;
        cameraTransform.localPosition = camPos;
    }

    void Update()
    {
        if (currentState == State.Climbing)
            HandleClimbing();
        else
            HandleWalking();
    }

    void HandleWalking()
    {
        HandleCrouch();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = isCrouching ? crouchSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleClimbing()
    {
        // Cancel crouch when climbing
        isCrouching = false;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Climb up/down with W/S, strafe on ladder with A/D
        Vector3 climbMove = transform.up * vertical + transform.right * horizontal;
        controller.Move(climbMove * climbingSpeed * Time.deltaTime);

        // Kill gravity while on ladder
        velocity = Vector3.zero;
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = true;
        if (Input.GetKeyUp(KeyCode.LeftControl) && CanStandUp())
            isCrouching = false;

        float targetHeight = isCrouching ? crouchHeight : standHeight;

        if (!Mathf.Approximately(controller.height, targetHeight))
        {
            if (Mathf.Abs(controller.height - targetHeight) > 0.001f)
                controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
            else
                controller.height = targetHeight;

            controller.center = new Vector3(0, controller.height / 2f, 0);
        }

        float targetCameraY = isCrouching ? crouchCameraY : standCameraY;
        Vector3 camPos = cameraTransform.localPosition;

        if (Mathf.Abs(camPos.y - targetCameraY) > 0.001f)
            camPos.y = Mathf.Lerp(camPos.y, targetCameraY, crouchTransitionSpeed * Time.deltaTime);
        else
            camPos.y = targetCameraY;

        cameraTransform.localPosition = camPos;
    }

    void EnterLadder()
    {
        currentState = State.Climbing;
        velocity = Vector3.zero;
    }

    void ExitLadder()
    {
        currentState = State.Walking;
        isOnLadder = false;
    }

    // Called by trigger on the ladder object
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
            EnterLadder();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
            ExitLadder();
    }

    bool CanStandUp()
    {
        Vector3 castOrigin = transform.position + Vector3.up * controller.height;
        float castDistance = standHeight - crouchHeight;
        return !Physics.SphereCast(castOrigin, controller.radius * 0.9f, Vector3.up, out _, castDistance);
    }
}