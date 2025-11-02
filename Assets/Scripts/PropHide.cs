using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class PropHide : MonoBehaviour
{
    [SerializeField] private bool touching_player = false;
    private GameObject player;
    [SerializeField] public float tub_depth = 0.3f;
    [SerializeField] public float y_rotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is 

    private float unhide_cooldown = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("touching_player: " + touching_player + " Pressing E: " + Input.GetKeyDown(KeyCode.E) + " not hiding: " + !player.GetComponent<PlayerMovement>().is_hiding);
        // if currently touching player AND player presses 'e' AND player is currently not hiding
        if (touching_player && Input.GetKeyDown(KeyCode.E))
        {
            if (!player.GetComponent<PlayerMovement>().is_hiding && unhide_cooldown <= 0f)
            {
                // let the player hide
                Vector3 hide_position = gameObject.transform.position;
                hide_position -= new Vector3(0f, tub_depth, 0f);
                player.GetComponent<PlayerMovement>().Hide(hide_position, new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y - y_rotation, gameObject.transform.rotation.z), gameObject);
            }
        }

        if (unhide_cooldown > 0)
        {
            unhide_cooldown -= 0.01f;
        }
    }   

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player = collision.gameObject;
        }
        if (collision.gameObject.name == "PlayerCapsule")
        {
            
            touching_player = true;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "PlayerCapsule")
        {
            touching_player = false;
        }
    }

    public void StartTimer()
    {
        unhide_cooldown = 1f;
    }
}
