// using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;


public class Sound : MonoBehaviour {
    public PlayerMovement PlayerMovement;
    public AudioSource walking;
    // public AudioSource heartBeat;
    public float radius = 1f;
    public float pitch = 0.5f;
    public float currentR;
    public float changeR = 0.1f;
    public bool moving = false;
    public bool prevMoving = false;
    public bool nearEnemy = false;
    public bool hasEnemy;
    private ExMovement enemy;
    //public SphereCollider SphereCollider;
    void Start() {
    }
    // Update is called once per frame
    void Update() {
        // change radius size based on movement
        if (PlayerMovement.is_running)
        {
            currentR = radius + changeR;
        }
        else if ((PlayerMovement.is_crouching))
        {
            currentR = radius - changeR;
        }
        else
        {
            currentR = radius;
        }
        // Debug.Log("Current Radius: " + currentR);


        DetectEnemies();
        float input = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");

        if (input != 0) {
            moving = true;
        } else {
            moving = false;
        }

        if (!prevMoving && moving) {
            prevMoving = true;
            walking.Play();

        } else if (!moving && prevMoving) {
            moving = false;
            walking.Stop();
        }
        prevMoving = moving;    
        walking.pitch = pitch * currentR / radius;

        if (!nearEnemy && hasEnemy) {
            nearEnemy = true;
            // heartBeat.Play();
            enemy.navAgent.SetDestination(transform.position);
        } else if (nearEnemy && !hasEnemy) {
            nearEnemy = false;
            // heartBeat.Stop();
        }

    }
    void DetectEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, currentR);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log("Enemy detected: " + hit.name);
                if (!hasEnemy)
                {
                    hasEnemy = true;
                    enemy = hit.GetComponent<ExMovement>();
                    break;
                }
            }
            hasEnemy = false;
        }
    }
    
    // Visualize the sphere in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentR);
    }
}
