using UnityEngine;


public class Sound : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public float radius = 1f;
    public float currentR;
    public float changeR = 0.1f;
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
            // the actual change in radius
/*        if (SphereCollider != null) {
            SphereCollider.radius = currentR;
           // Debug.Log(radius);
        }*/
        DetectEnemies();

      
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
