using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Canvas hud;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Canvas gameplayCanvas;

    private DepthOfField depthOfField;
    private bool isPaused = false;
    private bool isOnOptionsMenu = false;

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
            gameplayCanvas.gameObject.SetActive(true);
        }
        Time.timeScale = 1f;
        LockCursor();
    }

    void Update()
    {
        // Detect when the user presses the Tab key
        if (Input.GetKeyDown(KeyCode.Escape))
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
            gameplayCanvas.gameObject.SetActive(!isPaused);
        }

        if (!isPaused)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }

        // Optional: Freeze time when paused, unfreeze when playing
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ToggleOptionsMenu()
    {
        isOnOptionsMenu = !isOnOptionsMenu;
        // Toggle the UI Canvas visibility using gameobject activation
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(!isOnOptionsMenu);
        }
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(isOnOptionsMenu);
        }
    }

    void LockCursor(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnlockCursor(){
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OnPlayPressed(){
        ToggleHUD();
    }

    public void OnRestartLvlPressed(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void OnQuitPressed(){
        SceneManager.LoadScene("TitleScreeen");
        
    }
}