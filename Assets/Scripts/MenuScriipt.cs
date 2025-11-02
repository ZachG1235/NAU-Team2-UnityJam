using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScriipt : MonoBehaviour
{
    public Slider slider;

    [SerializeField]
    private Text sensText;
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
        print("play");
        //go to the correct scene
        SceneManager.LoadScene("IntroCutscene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnSliderValueChanged(float newValue)
    {
        Debug.Log("Slider value changed to: " + newValue);
        sensText.text = newValue.ToString();
        sensitivity.mouse_sensitivity = newValue;
    }
}
