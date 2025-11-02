using UnityEngine;

public class waypoint : MonoBehaviour
{   
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ex"))
        {
            other.GetComponent<ExMovement>().StartCoroutine("WaypointReached");
        }
    }
}
