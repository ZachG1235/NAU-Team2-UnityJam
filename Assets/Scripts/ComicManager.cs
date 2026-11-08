using System.Collections;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    // This class lets you define how to make the camera "step" through the panels of a comic
    [System.Serializable]
    public class CameraStep
    {
        [Header("Camera Positioning")]
        public float x;
        public float y;
        public float size;

        [Header("Timing")]
        public float LerpDuration = 0.5f;
        public float WaitDuration = 0f;

        [Header("Audio")]
        public AudioClip sfx;
    }

    [Header("Refrences")]
    [SerializeField] private Camera cam;
    [SerializeField] private AudioSource audioSource;

    // Cam is short for Camera for those new to the stream
    [Header("Cam Start")]
    [SerializeField] private Vector2 startPosition = new Vector2(-6.6f, 14.3f);
    [SerializeField] private float startSize = 3.5f;

    [Header("Sequence")]
    [SerializeField] private CameraStep[] steps = IntroSteps();

    private void Reset()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        steps = IntroSteps();
    }

    [ContextMenu("Load Default Steps")]
    private void LoadIntroSteps() => steps = IntroSteps();

    private void Start()
    {
        if (cam == null) cam = Camera.main;

        cam.transform.position = new Vector3(startPosition.x, startPosition.y, cam.transform.position.z);
        cam.orthographicSize = startSize;

        Play();
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
       foreach (CameraStep step in steps)
        {
            yield return RunStep(step);
        }
    }

    private IEnumerator RunStep(CameraStep step)
    {
        Vector3 startPos = cam.transform.position;
        float fromSize = cam.orthographicSize;

        Vector3 endPos = new Vector3( step.x, step.y, startPos.z);
        float endSize = step.size;

        if (step.LerpDuration > 0f)
        {
            float time = 0f;
            while (time < step.LerpDuration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / step.LerpDuration);

                cam.transform.position = Vector3.Lerp(startPos, endPos, t);
                cam.orthographicSize = Mathf.Lerp(fromSize, endSize, t);
                yield return null;
            }
            cam.transform.position = endPos;
            cam.orthographicSize = endSize;
        }

        if (step.sfx != null && audioSource != null)
            audioSource.PlayOneShot(step.sfx);

        if (step.WaitDuration > 0f)
            yield return new WaitForSeconds(step.WaitDuration);
    }


    private static CameraStep[] IntroSteps()
    {
        return new CameraStep[]
        {
            new CameraStep {x = -6.6f, y = 10.8f, size = 3.5f,
                    LerpDuration = 2f, WaitDuration = 0f},

            new CameraStep {x = 4.8f, y = 13.1f, size = 4f,
                    LerpDuration = 0.5f, WaitDuration = 1.5f},

            new CameraStep {x = 6.7f, y = 2.4f, size = 3.5f,
                    LerpDuration = 0.5f, WaitDuration = 0f},

            new CameraStep {x = 6.7f, y = -1.8f, size = 3.5f,
                    LerpDuration = 1.5f, WaitDuration = 0f},

            new CameraStep {x = -5.2f, y = .6f, size = 4.5f,
                    LerpDuration = 0.5f, WaitDuration = 1.5f},

            new CameraStep {x = -4.8f, y = -10f, size = 4f,
                    LerpDuration = 0.5f, WaitDuration = 0f},

            new CameraStep {x = -4.8f, y = -14f, size = 4f,
                    LerpDuration = 1.5f, WaitDuration = 0f},
        };
    }
}

