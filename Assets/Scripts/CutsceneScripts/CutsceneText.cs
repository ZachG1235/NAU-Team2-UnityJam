using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class CutsceneText : MonoBehaviour
{
    public List<string> textLines = new List<string>();  // list of dialogue lines
    public TextMeshProUGUI textBox;                     // assign in Inspector
    public float typingSpeed = 0.05f;
    public float initialDelay = 1f;
    public float delayBetweenLines = 1f;               // wait before next line

    private void Start()
    {
        if (textLines.Count > 0)
            StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(initialDelay);

        foreach (string line in textLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(delayBetweenLines);
        }
    }

    private IEnumerator TypeLine(string line)
    {
        textBox.text = "";
        foreach (char letter in line)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
