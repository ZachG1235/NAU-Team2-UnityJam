using UnityEngine;

public class FloatCam : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Camera cam;

    [Header("Bounds")]
    [SerializeField] private float top = 40.8f;
    [SerializeField] private float bot = 37.2f;
    [SerializeField] private float left = -2.5f;
    [SerializeField] private float right = 2.3f;

    [Header("Movement")]
    [SerializeField] private float changeOnInterval = 3f;
    [SerializeField] private float smoothing = 2f;
    [SerializeField] private float wobblePos = .25f;
    [SerializeField] private float wobbleSpeed = .5f;
    private Vector2 basePos;
    private Vector2 target;
    private Vector2 velocity;
    private float timer;
    private float noiseX, noiseY;

    private void Reset()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        if (cam == null) cam = Camera.main;

        basePos = cam. transform.position;
        noiseX = Random.value * 100f;
        noiseY = Random.value * 100f;
        PickNewTarget();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeOnInterval)
        {
            PickNewTarget();
        }

        basePos = Vector2.SmoothDamp(basePos, target, ref velocity, smoothing);
        float t = Time.time * wobbleSpeed;
        float wobbleX = (Mathf.PerlinNoise(noiseX, t) - 0.5f) * 2f * wobblePos;
        float wobbleY = (Mathf.PerlinNoise(noiseY, t) - 0.5f) * 2f * wobblePos;
        // Clamp
        float x = Mathf.Clamp(basePos.x + wobbleX, left, right);
        float y = Mathf.Clamp(basePos.y + wobbleY, bot, top);

        cam.transform.position = new Vector3(x, y, cam.transform.position.z);
    }

    private void PickNewTarget()
    {
        target = new Vector2(Random.Range(left, right), Random.Range(bot, top));
        timer = 0f;
    }


}