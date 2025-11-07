using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class countKey : MonoBehaviour
{
    public int numOfKeys = 3;
    public GameObject door;
    public GameObject TPAway;
    public GameObject player;
    [SerializeField] private bool touching_player = false;

    public TextMeshProUGUI keyText;
    private int numKeys;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numKeys = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Num of keys: "+ numOfKeys);
        if (numOfKeys == 0) {
            if (touching_player && Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene("WinCutscene");
            }
        }
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
    public void deleteKey() { 
        //update key text
        numKeys++;
        keyText.text = "Goal: " + numKeys + "/3 Keys Found";

        numOfKeys --;
        print("deleted key");
    }
}
