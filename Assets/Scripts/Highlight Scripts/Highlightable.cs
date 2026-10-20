using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Put this on any object the player can interact with.
/// Call SetHighlighted(true/false) to turn the highlight on or off.
/// </summary>
public class Highlightable : MonoBehaviour
{
    public enum Mode
    {
        Tint,            // Blends the object's color toward the highlight color
        OverlayMaterial, // Adds an extra material on top (needs a custom outline shader)
        ScaledCopy       // Outline made from a slightly bigger copy of the mesh (no custom shader needed)
    }

    public TMP_Text hoverText;

    [SerializeField] private Mode mode = Mode.ScaledCopy;

    [Header("Tint mode")]
    [SerializeField] private Color highlightColor = new Color(1f, 0.9f, 0.3f);
    [SerializeField, Range(0f, 1f)] private float tintStrength = 0.5f;

    [Header("Overlay / Scaled Copy modes")]
    [Tooltip("Overlay mode: an outline shader material.\nScaled Copy mode: a URP Unlit material with Render Face set to Back.")]
    [SerializeField] private Material overlayMaterial;

    [Header("Scaled Copy mode")]
    [Tooltip("How much bigger the outline copy is. 1.05 = 5% bigger.")]
    [SerializeField] private float outlineScale = 1.05f;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP / HDRP
    private static readonly int ColorId = Shader.PropertyToID("_Color");         // Built-in pipeline

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Color[] originalColors;
    private int[] colorPropertyIds;
    private MaterialPropertyBlock block;
    private readonly List<GameObject> outlineCopies = new List<GameObject>();

    public bool IsHighlighted { get; private set; }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];
        originalColors = new Color[renderers.Length];
        colorPropertyIds = new int[renderers.Length];
        block = new MaterialPropertyBlock();

        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] mats = renderers[i].sharedMaterials;
            originalMaterials[i] = mats;

            Material first = mats.Length > 0 ? mats[0] : null;
            if (first != null && first.HasProperty(BaseColorId))
            {
                colorPropertyIds[i] = BaseColorId;
                originalColors[i] = first.GetColor(BaseColorId);
            }
            else if (first != null && first.HasProperty(ColorId))
            {
                colorPropertyIds[i] = ColorId;
                originalColors[i] = first.GetColor(ColorId);
            }
            else
            {
                colorPropertyIds[i] = -1;
            }
        }

        if (mode == Mode.ScaledCopy) BuildOutlineCopies();
    }

    private void BuildOutlineCopies()
    {
        if (overlayMaterial == null)
        {
            Debug.LogWarning($"{name}: Scaled Copy mode needs a material in the Overlay Material slot.", this);
            return;
        }

        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            MeshFilter mf = mr.GetComponent<MeshFilter>();
            if (mf == null || mf.sharedMesh == null) continue;

            GameObject copy = new GameObject("OutlineCopy");
            copy.transform.SetParent(mr.transform, false);
            copy.transform.localScale = Vector3.one * outlineScale;
            copy.layer = mr.gameObject.layer;

            copy.AddComponent<MeshFilter>().sharedMesh = mf.sharedMesh;
            MeshRenderer outlineRenderer = copy.AddComponent<MeshRenderer>();
            outlineRenderer.sharedMaterial = overlayMaterial;
            outlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            outlineRenderer.receiveShadows = false;

            copy.SetActive(false);
            outlineCopies.Add(copy);
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        if (highlighted == IsHighlighted) return;
        IsHighlighted = highlighted;

        // Show text when highlighted, hide it when not
        if (hoverText != null)
            hoverText.gameObject.SetActive(highlighted);

        switch (mode)
        {
            case Mode.Tint: ApplyTint(highlighted); break;
            case Mode.OverlayMaterial: ApplyOverlay(highlighted); break;
            case Mode.ScaledCopy: ApplyScaledCopy(highlighted); break;
        }
    }

    private void ApplyScaledCopy(bool on)
    {

        foreach (GameObject copy in outlineCopies)
            if (copy != null) copy.SetActive(on);
    }

    private void ApplyTint(bool on)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (colorPropertyIds[i] == -1) continue;

            if (on)
            {
                renderers[i].GetPropertyBlock(block);
                block.SetColor(colorPropertyIds[i],
                    Color.Lerp(originalColors[i], highlightColor, tintStrength));
                renderers[i].SetPropertyBlock(block);
            }
            else
            {
                renderers[i].SetPropertyBlock(null);
            }
        }
    }

    private void ApplyOverlay(bool on)
    {
        if (overlayMaterial == null)
        {
            Debug.LogWarning($"{name}: Overlay mode selected but no overlay material assigned.", this);
            return;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (on)
            {
                Material[] original = originalMaterials[i];
                Material[] withOverlay = new Material[original.Length + 1];
                original.CopyTo(withOverlay, 0);
                withOverlay[original.Length] = overlayMaterial;
                renderers[i].sharedMaterials = withOverlay;
            }
            else
            {
                renderers[i].sharedMaterials = originalMaterials[i];
            }
        }
    }

    private void OnDisable()
    {
        SetHighlighted(false);
    }
}