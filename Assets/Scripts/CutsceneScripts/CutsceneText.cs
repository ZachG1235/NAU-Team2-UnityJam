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
    public AudioSource text_bloop_sfx;
    public string introText;
    public GameObject introTextBox;
    public bool isTyping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(lvl != -1){
            StartCoroutine(DisplayText());
        }
        else{
            if(introTextBox != null){
                introTextBox.SetActive(true);
                StartCoroutine(TypeText(introText));
            }
        }
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
        isTyping = true;
        int text_index = 0;
        yield return new WaitForSeconds(initialDelay);
        textBox.text = ""; // start empty

        foreach (char letter in text.ToCharArray())
        {
            textBox.text += letter; // add one letter
            if (text_index % 2 == 1)
            {
                text_bloop_sfx.Play();
                float[] pitch_array = { 0.95f, 1f, 1.05f };
                text_bloop_sfx.pitch = pitch_array[Random.Range(0, 2)];
            }
            text_index++;
            yield return new WaitForSeconds(typingSpeed); // wait
            
        }

        if(lvl == 1)
        {
            yield return new WaitForSeconds(ending_seconds_wait_time);
            FinishCutscene();
        }

        if(introTextBox != null)
        {
            yield return new WaitForSeconds(ending_seconds_wait_time/2);
            introTextBox.SetActive(false);
        }

    }

    public void FinishCutscene()
    {
        SceneManager.LoadScene("Level");
        
    }



    
}