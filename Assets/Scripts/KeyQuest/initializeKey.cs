using System.Collections.Generic;
using System.Net;
using UnityEngine;
using TMPro;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;


public class initializeKey : MonoBehaviour
{
    public GameObject[] keysSpots;
    public List<GameObject> spots = new List<GameObject>();
    public GameObject[] keys;
    [SerializeField] private bool touching_player = false;
    private GameObject player;
    private int spotAmount;
    CapsuleCollider capsuleCollider;

    [Header("Text Stuff")]
    public GameObject canvasObject;
    public TextMeshProUGUI textBox;
    public TextMeshProUGUI goalText;
    public string words;
    public float textTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spotAmount = keysSpots.Length;
        foreach (GameObject spot in keysSpots) {
            spots.Add(spot);
        }
        capsuleCollider = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (touching_player && Input.GetKeyDown(KeyCode.E)) {
            showKeys();
            goalText.text = "Goal: 0/3 Keys Found";
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

    void showKeys() {
        foreach (GameObject key in keys) {
            int randomNumberInt = Random.Range(0, spotAmount);
            key.transform.position = (spots[randomNumberInt]).transform.position;
            spots.Remove(spots[randomNumberInt]);
            spotAmount--;

        }
        StartCoroutine(ItemAppear());

        enabled = false;

        
    }

    System.Collections.IEnumerator ItemAppear() {
        canvasObject.SetActive(true);
        textBox.text = words;
        yield return new WaitForSeconds(textTimer);
        canvasObject.SetActive(false);
    }

}
