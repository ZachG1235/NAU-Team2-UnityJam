using UnityEngine;

public class countKey : MonoBehaviour
{
    public int numOfKeys = 3;
    public GameObject door;
    public GameObject TPAway;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Num of keys: "+ numOfKeys);
        if (numOfKeys == 0) {
            door.transform.position = TPAway.transform.position;   
        }
    }

    public void deleteKey() { 
        numOfKeys --;
    }
}
