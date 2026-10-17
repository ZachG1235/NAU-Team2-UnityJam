using UnityEngine;

/// <summary>
/// Put this on the player (or camera). Every frame it casts a ray from the
/// camera and highlights whichever Highlightable it's pointing at.
/// </summary>
public class InteractionHighlighter : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayers = ~0; // Everything by default

    private Highlightable current;

    private void Awake()
    {
        if (playerCamera == null) playerCamera = Camera.main;
    }

    private void Update()
    {
        Highlightable target = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayers,
                QueryTriggerInteraction.Ignore))
        {
            target = hit.collider.GetComponentInParent<Highlightable>();
        }

        if (target == current) return;

        if (current != null) current.SetHighlighted(false);
        current = target;
        if (current != null) current.SetHighlighted(true);
    }

    private void OnDisable()
    {
        if (current != null) current.SetHighlighted(false);
        current = null;
    }

    /// <summary>The object currently being looked at (use this for your interact key).</summary>
    public Highlightable Current => current;
}