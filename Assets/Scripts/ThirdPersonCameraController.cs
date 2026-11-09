using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private InputActionReference lookAction;

    [Header("Camera")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float heightOffset = 1.5f;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;

    private float yaw;
    private float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target != null)
        {
            yaw = transform.eulerAngles.y;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if (lookAction != null)
            lookAction.action.Enable();
    }

    private void OnDisable()
    {
        if (lookAction != null)
            lookAction.action.Disable();
    }

    private void LateUpdate()
    {
        if (target == null || lookAction == null)
            return;

        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + Vector3.up * heightOffset;

        transform.position = focusPoint - rotation * Vector3.forward * distance;
        transform.rotation = rotation;
    }
}
