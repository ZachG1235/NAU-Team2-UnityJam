using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class ExMovement : MonoBehaviour
{
    [Header("Component References")]
    public NavMeshAgent navAgent;
    public Animator animator;
    public FieldOfView fov;

    public float walkSpeed = 1f;
    public float chaseSpeed = 1.2f;
    public bool isChasing = false;
    public GameObject player_seen;
    private AudioSource seen_audio;
    private float seen_sound_cooldown = 1f;

    [Header("Patrol Waypoints; in sequenctial order")]
    [Tooltip("Place waypoints in scene and assign them here")]
    public List<Transform> waypoints;
    // public float idleTime;   // time to idle at each waypoint; making random instead
    private int currentWaypointIndex = 0;
    private int tempIndex,waypointCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent.SetDestination(waypoints[0].position);
        waypointCount = waypoints.Count;
        navAgent.speed = walkSpeed;
        seen_audio = player_seen.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if can see player
        if (fov.canSeePlayer)
        {
            // CHASEEEEEEE
            isChasing = true;
            navAgent.speed = chaseSpeed;
            navAgent.SetDestination(fov.playerRef.transform.position);
            animator.Play("Walking_A 0");
            if (seen_sound_cooldown < 0)
            {
                seen_sound_cooldown = 1f;
                seen_audio.Play();
            }
            else if (seen_sound_cooldown < 0.10f)
            {
                seen_audio.Stop();
            }
        }
        // otherwise, assume was chasing but cant see player
        else if (isChasing && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            Debug.Log("Lost sight of player, returning to patrol");
            // stop chasing, go to nearest waypoint
            isChasing = false;
            navAgent.speed = walkSpeed;

            // find nearest waypoint
            float nearestDist = Vector3.Distance(transform.position, waypoints[0].position);
            int nearestIndex = 0;
            for (int i = 1; i < waypointCount; i++)
            {
                float dist = Vector3.Distance(transform.position, waypoints[i].position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestIndex = i;
                }
            }

            // set destination to nearest waypoint
            currentWaypointIndex = nearestIndex;
            
            // wait a few seconds and decide next waypoint
            StartCoroutine("WaypointReached");
        }
        
    }

    void FixedUpdate()
    {
        seen_sound_cooldown -= 0.01f;
    }
    
    public IEnumerator WaypointReached()
    {
        isChasing = false;
            navAgent.speed = walkSpeed;

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
            StartCoroutine("WaypointReached");
        }
        else
        {
            // move to next waypoint
            navAgent.SetDestination(waypoints[currentWaypointIndex].position);

            // play walk aimation
            animator.Play("Walking_A 0");
        }
    }
}
