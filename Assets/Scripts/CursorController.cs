using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera
    public LayerMask groundLayer;  // Layer mask for the ground
    public float hoverHeight = 1.0f;  // Height at which the cursor will hover above the ground
    public float cursorSensitivity = 0.1f;  // Sensitivity for cursor movement

    private Vector3 virtualCursorPosition;

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize virtualCursorPosition at a set distance in front of the camera
        virtualCursorPosition = mainCamera.transform.position + mainCamera.transform.forward * 10f;  // Default cursor distance
    }

    void Update()
    {
        // Calculate virtual cursor movement
        float moveHorizontal = Input.GetAxis("Mouse X") * cursorSensitivity;
        float moveVertical = Input.GetAxis("Mouse Y") * cursorSensitivity;

        virtualCursorPosition += new Vector3(moveHorizontal, 0, moveVertical);

        // Raycast from the virtual cursor position to the ground
        Ray ray = mainCamera.ScreenPointToRay(virtualCursorPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y += hoverHeight;  // Add hover height to the cursor's position
            transform.position = hitPoint;

            // Ensure the cursor faces the camera
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, Vector3.up);
        }
    }
}
