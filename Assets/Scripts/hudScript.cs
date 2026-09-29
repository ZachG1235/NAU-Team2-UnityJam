using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class hudScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Canvas hud;
    [SerializeField] private Canvas gameplayCanvas;

    private DepthOfField depthOfField;
    private bool isPaused = false;

    void Start()
    {
        // Get the Depth of Field component from the Volume profile
        if (globalVolume != null && globalVolume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            // Start with blur turned off
            depthOfField.active = false; 
        }
        else
        {
            Debug.LogError("No Depth of Field override found on the Global Volume profile or Global Volume is missing!");
        }

        // Ensure HUD starts hidden at the beginning of the game
        if (hud != null)
        {
            hud.gameObject.SetActive(false);
        }
        if (gameplayCanvas != null)
        {
            gameplayCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Detect when the user presses the Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleHUD();
            Debug.Log("HUD toggled. Current pause state: " + isPaused);
        }
    }

    void ToggleHUD()
    {
        // Flip the state (if it was false, it becomes true, and vice versa)
        isPaused = !isPaused;

        // Toggle the Blur effect
        if (depthOfField != null)
        {
            depthOfField.active = isPaused; 
        }

        // Toggle the UI Canvas visibility using gameobject activation
        if (hud != null)
        {
            hud.gameObject.SetActive(isPaused);
        }
        if (gameplayCanvas != null)
        {
            gameplayCanvas.gameObject.SetActive(isPaused);
        }

        // Optional: Freeze time when paused, unfreeze when playing
        Time.timeScale = isPaused ? 0f : 1f;
    }
}