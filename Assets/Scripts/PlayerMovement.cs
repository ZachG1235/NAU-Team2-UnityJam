using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Speed")]
    public float speed = 6f;
    public float sprint_multiplier = 1.5f;
    public float crouch_multiplier = 0.5f;
    public float mouseSensitivity = 100f;
    

    public bool is_running = false;
    public bool is_crouching = false;

    [Header("Player Camera")]
    public Transform playerCamera;
    public float standing_camera_height = 0.6f;
    public float crouched_camera_height = 0.35f;
    public float camera_change_rate = 1f;


    float verticalRotation = 0f;
    CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update()
    {
        GetPlayerStatus();
        MovePlayer();
        LookAround();
        TransitionCamera();
        // Debug.Log("Is running = " + is_running + " is crouch = " + is_crouching);
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float move_multiplier = 1f;
        if (is_running)
        {
            move_multiplier = sprint_multiplier;
        }
        else if (is_crouching)
        {
            move_multiplier = crouch_multiplier;
        }

        controller.SimpleMove(move * (speed*move_multiplier));
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void GetPlayerStatus()
    {
        is_running = Input.GetKey(KeyCode.LeftShift);
        is_crouching = Input.GetKey(KeyCode.LeftControl);
        
        if (is_running && is_crouching)
        {
            is_running = false;
        }
    }

    void TransitionCamera()
    {
        if (is_crouching && (playerCamera.transform.localPosition.y > crouched_camera_height))
        {
            // move camera to crouched y position

            playerCamera.transform.localPosition += Vector3.down * camera_change_rate * Time.deltaTime;
        }
        else if (!is_crouching && (playerCamera.transform.localPosition.y < standing_camera_height))
        {
            // move camera to standing y position
            playerCamera.transform.localPosition += Vector3.up * camera_change_rate * Time.deltaTime;
        }
    }

}
