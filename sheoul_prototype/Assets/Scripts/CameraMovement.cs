using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // "To Who Belongs the Camera?"
    [SerializeField]
    private Transform character;

    // Default Value for Sensitivity
    [SerializeField]
    private float mouseSensitivity = 75f;

    // Default Mouse "Look"
    bool mouseInverted = false;

    // Default "xRotation" Value
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Hides Mouse Pointer
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = 
            Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        float mouseY = 
            Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        /* 
         * Checks Player's Mouse "Look" Definition and Adapts Camera's 
         * Reactions to is commands
         */   
        if (mouseInverted == true)
        {
            // Inverted Commands
            xRotation += mouseY;
        }
        else
        {
            // Default Commands
            xRotation -= mouseY;
        }

        // Limitations to Character's Camera Rotation
        xRotation = Mathf.Clamp(xRotation, -75f, 60f);

        // Character's Rotation (with Camera)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        character.Rotate(Vector3.up * mouseX);
    }
}
