using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class grabLogic : MonoBehaviour
{
    [SerializeField] private bool touching_player = false;
    public float timer = 100f;
    public GameObject player;
    public GameObject newModel;
    public GameObject hold;
    public GameObject teleportAway;
    public bool curInterecting = false;
    public PlayerMovement playerMovement;
    public GameObject textBox;
    public countKey countKey;

    //text stuff
    public TextMeshProUGUI text;
    public string words;

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
        textBox.SetActive(true);
        text.text = words;
        
        Vector3 targetPosition = new Vector3(hold.transform.position.x, hold.transform.position.y, hold.transform.position.z);
        //player.transform.LookAt(targetPosition);
        countKey.deleteKey();
        yield return new WaitForSeconds(timer);
        playerMovement.enabled = true;
        newModel.transform.position = teleportAway.transform.position;
        curInterecting = false;
        textBox.SetActive(false);
    }
}
