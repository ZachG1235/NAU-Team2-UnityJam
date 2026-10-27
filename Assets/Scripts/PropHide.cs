using UnityEngine;

public class PropHide : MonoBehaviour
{
    [SerializeField] public float hideDistance = 3.5f;
    [SerializeField] public float tub_depth = 0.3f;
    [SerializeField] public float y_rotation = 0f;

    private GameObject player;
    private PlayerMovement playerMovement;
    private float unhide_cooldown = 1f;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (playerMovement.is_hiding || unhide_cooldown > 0f) return;

        Transform cam = playerMovement.playerCamera;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, hideDistance)
            && hit.transform.IsChildOf(transform))
        {
            Vector3 hide_position = transform.position - new Vector3(0f, tub_depth, 0f);
            Vector3 rotation = new Vector3(0f, transform.eulerAngles.y - y_rotation, 0f);
            playerMovement.Hide(hide_position, rotation, gameObject);
        }
    }

    void FixedUpdate()
    {
        if (unhide_cooldown > 0) unhide_cooldown -= 0.01f;
    }

    public void StartTimer()
    {
        unhide_cooldown = 1f;
    }
}