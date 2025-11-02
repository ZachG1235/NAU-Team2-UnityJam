using UnityEngine;

public class HoverObject : MonoBehaviour {
    private HoverManager hoverManager;
    [SerializeField] private bool touching_player = false;
    [SerializeField] private bool mouseEnter = false;
    public GameObject player;
    void Start() {
        hoverManager = FindFirstObjectByType<HoverManager>();
        // Finds the UI manager
    }
    void Update() {
        if (touching_player && mouseEnter) {
            Debug.Log("ahhhhhh");
            hoverManager.ShowHoverText(transform, "Press E");
        }
        if (!touching_player || !mouseEnter) {
            hoverManager.HideHoverText();
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
    void OnMouseEnter() {
        Debug.Log("Enter");
        mouseEnter = true;



    }

    void OnMouseExit() {
        Debug.Log("exit");

        mouseEnter = false;

    }
}