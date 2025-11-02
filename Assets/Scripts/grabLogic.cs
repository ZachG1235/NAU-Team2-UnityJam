using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class grabLogic : MonoBehaviour
{
    [SerializeField] private bool touching_player = false;
    public float timer = 100f;
    public bool timeStart = false;
    public GameObject player;
    public GameObject newModel;
    public GameObject hold;
    public GameObject teleportAway;
    public bool curInterecting = false;
    public PlayerMovement playerMovement;
    public GameObject canvas;
    public countKey countKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {

        if (touching_player && Input.GetKeyDown(KeyCode.E)) {
            if (!curInterecting) {
                interact();
            }
        }
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Player") {
            player = collision.gameObject;
        }
        if (collision.gameObject.name == "PlayerCapsule") {

            touching_player = true;
        }
    }
    void OnTriggerExit(Collider collision) {
        if (collision.gameObject.name == "PlayerCapsule") {
            touching_player = false;
        }
    }

    void interact() {
        curInterecting = true;
        StartCoroutine(ItemAppear());
    }

    IEnumerator ItemAppear() {
        transform.position = teleportAway.transform.position;

        newModel.transform.position = hold.transform.position;
        playerMovement.enabled = false;
        canvas.SetActive(true);

        countKey.deleteKey();
        yield return new WaitForSeconds(timer);
        playerMovement.enabled = true;
        newModel.transform.position = teleportAway.transform.position;
        curInterecting = false;
        canvas.SetActive(false);
    }
}
