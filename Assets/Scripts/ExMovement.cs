using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class ExMovement : MonoBehaviour
{
    [Header("Component References")]
    public NavMeshAgent navAgent;
    public Animator animator;

    [Header("Patrol Waypoints; in sequenctial order")]
    [Tooltip("Place waypoints in scene and assign them here")]
    public List<Transform> waypoints;
    // public float idleTime;   // time to idle at each waypoint; making random instead
    private int currentWaypointIndex = 0;
    private int tempIndex;
    private int waypointCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent.SetDestination(waypoints[0].position);
        waypointCount = waypoints.Count;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public IEnumerator Idle()
    {
        // play idle animation
        animator.Play("idle");

        // yield return new WaitForSeconds(idleTime);
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        // decide next waypoint
        // choose random waypoint near current waypoint
        tempIndex = currentWaypointIndex;
        currentWaypointIndex += Random.Range(-3, 4);

        // special cases for out of bounds indexes
        // using if statements cause apparently cant use switch with unconstant vars (sigh)
        if (currentWaypointIndex == -3)
        {
            currentWaypointIndex = waypointCount - 3;
        }
        else if (currentWaypointIndex == -2)
        {
            currentWaypointIndex = waypointCount - 2;
        }
        else if (currentWaypointIndex == -1)
        {
            currentWaypointIndex = waypointCount - 1;
        }
        else if (currentWaypointIndex == waypointCount)
        {
            currentWaypointIndex = 0;
        }
        else if (currentWaypointIndex == waypointCount + 1)
        {
            currentWaypointIndex = 1;
        }
        else if (currentWaypointIndex == waypointCount + 2)
        {
            currentWaypointIndex = 2;
        }

        // also checking if index stayed the same
        if (currentWaypointIndex == tempIndex)
        {
            StartCoroutine("Idle");
        }
        else
        {
            // move to next waypoint
            navAgent.SetDestination(waypoints[currentWaypointIndex].position);

            // play walk aimation
            animator.Play("walk");
        }
    }
}
