using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{
    public Slider slider;

    [SerializeField]
    private Text sensText;

    [Header("Scene Refrences")]
    [SerializeField] private GameObject floatCam;
    [SerializeField] private ComicManager comicManager;
    [SerializeField] private GameObject UICanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(slider != null){
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        // Changed for new cutscene
        if (floatCam != null) floatCam.SetActive(false);
        if (UICanvas != null) UICanvas.SetActive(false);
        if (comicManager != null) comicManager.Play();
    }

    public void Quit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("NewTitleScreen");
    }

    public void OnSliderValueChanged(float newValue)
    {
        Debug.Log("Slider value changed to: " + newValue);
        sensText.text = newValue.ToString();
        sensitivity.mouse_sensitivity = newValue;
    }
}
