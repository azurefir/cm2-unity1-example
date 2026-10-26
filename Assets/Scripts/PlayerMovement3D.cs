using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
// Everything above this is getting the things like import something in python

// Makes everything work
public class PlayerMovement3D : MonoBehaviour
{
    // Everything that has private somewhere in it is not a public/global variable or whatever
    [Header("Refrences")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Jump and Gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity;

    // When the game first runs and/or when the first object is placed
    private void Awake()
    { 
        controller = GetComponent<CharacterController>();
    }

    // When it is enabled
    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }
        if (jumpAction != null)
        {
            jumpAction.action.Enable();
        }
    }

    // When it is disabled
    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }
        if (jumpAction != null)
        {
            jumpAction.action.Disable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform == null || moveAction == null || jumpAction == null)
        {
            return;
        }

        // Movement and camera settings?
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 MoveDirection = cameraForward * input.y + cameraRight * input.x;

        MoveDirection = Vector3.ClampMagnitude(MoveDirection, 1f);

        // Camera movement or something
        if (MoveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // If the player is on the floor and not jumping the player is staying on the ground
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        // If the player is on the floor and the jump button was pressed the player jumps
        if (controller.isGrounded && jumpAction.action.WasPressedThisFrame())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Maybe making sure the gravity is always active (except when jumping)??
        verticalVelocity += gravity * Time.deltaTime;

        // Enabling collisions maybe? Or at least making them always possible??
        Vector3 velocity = MoveDirection * moveSpeed;
        velocity.y = verticalVelocity;

        CollisionFlags collisions = controller.Move(velocity * Time.deltaTime);

        // If the player is jumping and something hits it from above but the player is still jumping (> 0f) the player has no velocity (0f)
        if ((collisions & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
        {
            verticalVelocity = 0f;
        }
    }
}
