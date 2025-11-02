using UnityEngine;
using TMPro;

public class HoverManager : MonoBehaviour {
    public TMP_Text hoverText; // Assign HoverTextUI in the Inspector
    private Transform currentObject; // The object being hovered
    public Vector3 textOffset = new Vector3(0, 0, 0); // Adjust floating position
    public Camera cam;

    void Update() {
        if (currentObject != null) {
            // Move UI text to follow hovered object
            //hoverText.transform.position = cam.WorldToScreenPoint(currentObject.position + textOffset);
            //Debug.Log("text: "+hoverText.transform.position);
        }
    }

    public void ShowHoverText(Transform obj, string text) {
        currentObject = obj;
        hoverText.text = text;
        hoverText.gameObject.SetActive(true);
    }

    public void HideHoverText() {
        currentObject = null;
        hoverText.gameObject.SetActive(false);
    }
}