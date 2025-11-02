using UnityEngine;

public class HoverObject : MonoBehaviour {
    private HoverManager hoverManager;
    [SerializeField] private bool touching_player = false;
    public GameObject player;
    void Start() {
        hoverManager = FindFirstObjectByType<HoverManager>();
        // Finds the UI manager
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
        if (touching_player){
            hoverManager.ShowHoverText(transform, gameObject.name); }

    }

    void OnMouseExit() {
        Debug.Log("exit");
        if (touching_player) {
            hoverManager.HideHoverText();
        }

    }
}