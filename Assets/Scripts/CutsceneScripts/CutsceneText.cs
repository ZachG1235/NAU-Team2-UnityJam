using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class CutsceneText : MonoBehaviour
{
    public List<string> text = new List<string>();
    public TextMeshProUGUI textBox;
    public float typingSpeed = 0.05f;
    public float initialDelay = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TypeText(text[0]));
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
    }



    
}
