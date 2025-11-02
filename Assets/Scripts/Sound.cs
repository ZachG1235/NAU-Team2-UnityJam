using UnityEngine;


public class Sound : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public AudioSource audioSource;
    public float radius = 1f;
    public float pitch = 0.5f;
    public float currentR;
    public float changeR = 0.1f;
    public bool moving = false;
    public bool prevMoving = false;
    //public SphereCollider SphereCollider;
    void Start() { 
    }
    // Update is called once per frame
    void Update() {
        // change radius size based on movement
            if (PlayerMovement.is_running) {
                currentR = radius + changeR;
            } else if ((PlayerMovement.is_crouching)) {
                currentR = radius - changeR;

            } else {
                currentR = radius;
            }

    

        DetectEnemies();
        float input = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
        Debug.Log(input);
        if (input != 0) {
            moving = true;
        } else {
            moving = false;
        }

        if (!prevMoving && moving) {
            prevMoving = true;
            audioSource.Play();
            
        } else if (!moving && prevMoving) {
            moving = false;
            audioSource.Stop();
        }
        prevMoving = moving;
        audioSource.pitch = pitch * currentR / radius;
    }
    void DetectEnemies() {
        Collider[] hits = Physics.OverlapSphere(transform.position, currentR);
        foreach (Collider hit in hits) {
            if (hit.CompareTag("Enemy")) {
                Debug.Log("Enemy detected: " + hit.name);
            }
        }
    }
}
