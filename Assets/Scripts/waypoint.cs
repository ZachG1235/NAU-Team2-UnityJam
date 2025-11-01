using UnityEngine;

public class waypoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ex"))
        {
            other.GetComponent<ExMovement>().StartCoroutine("Idle");
        }
    }
}
