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
    public bool is_hiding = false;

    [Header("Player Camera")]
    public Transform playerCamera;
    public float camera_change_rate = 1f;
    public float standing_camera_height = 0.6f;
    public float crouched_camera_height = 0.3f;

    [Header("Camera Bob")]
    public bool enableBob = true;
    public float bobFrequency = 8f;            // how fast the bob cycles at walking speed
    public float bobAmplitude = 0.03f;         // vertical bob distance
    public float bobHorizontalAmplitude = 0.02f; // side-to-side sway
    public float bobSmoothing = 10f;           // how quickly the bob fades in/out

    private float bobTimer;
    private Vector3 bobOffset;
    private float currentCameraHeight;
    private Vector3 cameraBasePosition;

    [Header("Hiding Vars")]
    private Vector3 previous_location;
    private Quaternion previous_rotation;
    private float unhide_cooldown = 1f;
    private GameObject lastHidObject;


    float verticalRotation = 0f;
    CharacterController controller;
    public EnemyMovement enemy;
    public Sound soundScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mouseSensitivity = sensitivity.mouse_sensitivity;
        Cursor.lockState = CursorLockMode.Locked;

        // set camera base position and current height
        cameraBasePosition = playerCamera.localPosition;
        currentCameraHeight = cameraBasePosition.y;
    }
    
    // Update is called once per frame
    void Update()
    {
        GetPlayerStatus();
        if (!is_hiding)
        {
            MovePlayer();
        }
        else
        {
            if (unhide_cooldown < 0 && Input.GetKeyDown(KeyCode.E))
            {
                UnHide();
            }
        }
        LookAround();
        TransitionCamera();
        // Debug.Log("Is running = " + is_running + " is crouch = " + is_crouching);
    }

    void FixedUpdate()
    {
        // Debug.Log("Unhide Cooldown " + unhide_cooldown);
        if (is_hiding)
        {
            unhide_cooldown -= 0.01f;
        }
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
        // crouch / stand height
        float targetHeight = is_crouching ? crouched_camera_height : standing_camera_height;
        currentCameraHeight = Mathf.MoveTowards(currentCameraHeight, targetHeight, camera_change_rate * Time.deltaTime);

        // head bob
        Vector3 targetBob = Vector3.zero;

        if (enableBob && !is_hiding && controller.enabled && controller.isGrounded)
        {
            Vector3 flatVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
            float currentSpeed = flatVelocity.magnitude;

            if (currentSpeed > 0.1f)
            {
                // bob faster when sprinting, slower when crouching
                bobTimer += Time.deltaTime * bobFrequency * (currentSpeed / speed);

                targetBob = new Vector3(
                    Mathf.Cos(bobTimer) * bobHorizontalAmplitude,
                    Mathf.Sin(bobTimer * 2f) * bobAmplitude,
                    0f
                );
            }
        }

        // smooth back to zero when standing still
        bobOffset = Vector3.Lerp(bobOffset, targetBob, bobSmoothing * Time.deltaTime);

        playerCamera.localPosition = new Vector3(
            cameraBasePosition.x + bobOffset.x,
            currentCameraHeight + bobOffset.y,
            cameraBasePosition.z
        );
    }

    public void Hide(Vector3 teleport_position, Vector3 rotation, GameObject object_reference)
    {
        
        // save prior vectors
        previous_location = gameObject.transform.position;
        previous_rotation = gameObject.transform.rotation;
        lastHidObject = object_reference;

        is_hiding = true;

        // hide sound script (so that enemy won't try to chase you even if you're hiding)
        soundScript.enabled = false;

        controller.enabled = false;
        
        // apply rotation
        controller.transform.position = teleport_position;
        transform.rotation = Quaternion.Euler(rotation);
        
        // check if enemy sees player while trying to hide
        if (!enemy.fov.canSeePlayer)
        {
            enemy.StartCoroutine("WaypointReached");
        }
        unhide_cooldown = 1f;
    }

    public void UnHide()
    {
        gameObject.transform.position = previous_location;
        gameObject.transform.rotation = previous_rotation;
        lastHidObject.GetComponent<PropHide>().StartTimer();

        is_hiding = false;

        // enable sound script
        soundScript.enabled = true;

        controller.enabled = true;
    }

}
