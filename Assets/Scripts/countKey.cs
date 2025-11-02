using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class countKey : MonoBehaviour
{
    public int numOfKeys = 3;
    public GameObject door;
    public GameObject TPAway;
    public GameObject player;
    [SerializeField] private bool touching_player = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Num of keys: "+ numOfKeys);
        if (numOfKeys == 0) {
            if (touching_player && Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene("IntroCutscene");
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
        numOfKeys --;
    }
}
