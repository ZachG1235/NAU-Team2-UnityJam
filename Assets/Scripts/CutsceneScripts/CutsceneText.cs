using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneText : MonoBehaviour
{
    public List<string> text = new List<string>();
    public TextMeshProUGUI textBox;
    public float typingSpeed = 0.05f;
    public float initialDelay = 1f;
    public float ending_seconds_wait_time = 5f;
    public int lvl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisplayText());
        // Make cursor visible
        Cursor.visible = true;                    
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator DisplayText()
    {
        for(int i = 0; i < text.Count; i++)
        {
            StartCoroutine(TypeText(text[i]));
            yield return new WaitForSeconds(ending_seconds_wait_time);
        }
    }
    
    IEnumerator TypeText(string text)
    {
        yield return new WaitForSeconds(initialDelay);
        textBox.text = ""; // start empty

        foreach (char letter in text.ToCharArray())
        {
            textBox.text += letter; // add one letter
            yield return new WaitForSeconds(typingSpeed); // wait
        }

        if(lvl == 1)
        {
            yield return new WaitForSeconds(ending_seconds_wait_time);
            FinishCutscene();
        }
        
    }

    public void FinishCutscene()
    {
        SceneManager.LoadScene("Level");
        
    }



    
}