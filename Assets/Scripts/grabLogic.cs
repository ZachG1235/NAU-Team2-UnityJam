using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;

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

    //alien stuff
    public GameObject targetParent;  
    public int minToDestroy = 5;
    public int maxToDestroy = 15;

    //highlighting stuff
    public Camera playerCamera;
    public float hoverDistance = 4f;
    private Highlightable highlightable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highlightable = GetComponentInChildren<Highlightable>();
        if (highlightable == null)
            Debug.LogWarning($"{name}: no Highlightable found on key or its children.", this);
    }


    // Update is called once per frame
    void Update() {

    CheckHover();

        if (touching_player && Input.GetKeyDown(KeyCode.E)) {
            if (!curInterecting) {
                interact();
            }
        }
    }

    void CheckHover()
    {
        if (highlightable == null) return;

        bool hovering = false;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, hoverDistance, ~0, QueryTriggerInteraction.Collide))
        {
            hovering = hit.transform == transform || hit.transform.IsChildOf(transform);
        }

        highlightable.SetHighlighted(hovering && !curInterecting);
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

        DeAlienify();
    }

    IEnumerator ItemAppear() {
        transform.position = teleportAway.transform.position;

        newModel.transform.position = hold.transform.position;

        playerMovement.enabled = false;
        textBox.SetActive(true);
        text.text = words;
        
        Vector3 targetPosition = new Vector3(hold.transform.position.x, hold.transform.position.y, hold.transform.position.z);
        newModel.transform.LookAt(player.transform);
        countKey.deleteKey();
        yield return new WaitForSeconds(timer);
        playerMovement.enabled = true;
        newModel.transform.position = teleportAway.transform.position;
        curInterecting = false;
        textBox.SetActive(false);
    }

    void DeAlienify()
    {
        if (targetParent == null) return;

        // Collect only children that are exactly 2 levels deep
        List<GameObject> candidates = new List<GameObject>();
        foreach (Transform child in targetParent.transform)
        {
            foreach (Transform grandchild in child)
            {
                candidates.Add(grandchild.gameObject);
            }
        }

        if (candidates.Count == 0) return;

        // Pick a random amount to destroy (maxToDestroy is inclusive)
        int amount = UnityEngine.Random.Range(minToDestroy, maxToDestroy + 1);
        amount = Mathf.Min(amount, candidates.Count);

        for (int i = 0; i < amount; i++)
        {
            int index = UnityEngine.Random.Range(0, candidates.Count);
            Destroy(candidates[index]);
            candidates.RemoveAt(index); // prevents picking the same one twice
        }
    }
}
