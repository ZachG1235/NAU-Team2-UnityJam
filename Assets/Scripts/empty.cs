using UnityEngine;

public class empty : MonoBehaviour
{
    public float speed = 6f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    float verticalRotation = 0f;
    CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
