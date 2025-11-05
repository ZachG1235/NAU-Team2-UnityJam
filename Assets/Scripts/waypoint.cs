using UnityEngine;

public class waypoint : MonoBehaviour
{   
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.GetComponent<EnemyMovement>().isChasing)
        {
            other.GetComponent<EnemyMovement>().StartCoroutine("WaypointReached");
        }
    }
}
