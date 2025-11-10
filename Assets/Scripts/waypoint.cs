using UnityEngine;
using System.Collections;

public class waypoint : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        // check if
        // trigger is enemy
        // enemy is not chasing player
        // enemy was tring to go to this waypoint
        if (other.CompareTag("Enemy") && !other.GetComponent<EnemyMovement>().isChasing
            && other.GetComponent<EnemyMovement>().CheckWaypoint(this.gameObject))
        {
            Debug.Log("Enemy reached waypoint");
            other.GetComponent<EnemyMovement>().StartCoroutine("WaypointReached");
        }
    }
}
