using System.Collections;
using UnityEngine.SceneManagement;
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
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private float fadeOutTime = 3f;
    [SerializeField] private CanvasGroup fadePanel;


    // Cam is short for Camera for those new to the stream

    [Header("Sequence")]
    [SerializeField] private CameraStep[] steps = IntroSteps();

    private void Reset()
    {
        cam = Camera.main;
        audioSourceSFX = GetComponent<AudioSource>();
        steps = IntroSteps();
    }

    [ContextMenu("Load Default Steps")]
    private void LoadIntroSteps() => steps = IntroSteps();

    private void Start()
    {
        if (cam == null) cam = Camera.main;
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

        yield return FadeOut();

        // Everything is Done! Load game!
        SceneManager.LoadScene("IntroLevel");
    }

    private IEnumerator FadeOut()
    {
        if (audioSourceMusic == null || fadeOutTime <= 0f || fadePanel == null) yield break;

        float startVolume = audioSourceMusic.volume;
        float t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            float konstant = t / fadeOutTime;
            audioSourceMusic.volume = Mathf.Lerp(startVolume, 0f, konstant);
            fadePanel.alpha = konstant;
            yield return null;
        }
        audioSourceMusic.volume = 0f;
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

        if (step.sfx != null && audioSourceSFX != null)
            audioSourceSFX.PlayOneShot(step.sfx);

        if (step.WaitDuration > 0f)
            yield return new WaitForSeconds(step.WaitDuration);
    }


    private static CameraStep[] IntroSteps()
    {
        return new CameraStep[]
        {
            new CameraStep {x = -6.6f, y = 14.3f, size = 3.5f,
                    LerpDuration = 1.5f, WaitDuration = 1f},

            new CameraStep {x = -6.6f, y = 10.8f, size = 3.5f,
                    LerpDuration = 2f, WaitDuration = 1f},

            new CameraStep {x = 4.8f, y = 13.1f, size = 4f,
                    LerpDuration = 0.5f, WaitDuration = 2.5f},

            new CameraStep {x = 6.7f, y = 2.4f, size = 3.5f,
                    LerpDuration = 0.5f, WaitDuration = 0f},

            new CameraStep {x = 6.7f, y = -1.8f, size = 3.5f,
                    LerpDuration = 1.5f, WaitDuration = 1f},

            new CameraStep {x = -5.2f, y = .6f, size = 4.5f,
                    LerpDuration = 0.5f, WaitDuration = 2.5f},

            new CameraStep {x = -4.8f, y = -10f, size = 4f,
                    LerpDuration = 0.5f, WaitDuration = 0f},

            new CameraStep {x = -4.8f, y = -14f, size = 4f,
                    LerpDuration = 1.5f, WaitDuration = 1f},

            new CameraStep {x = 7f, y = -9f, size = 3.5f,
                    LerpDuration = 0.5f, WaitDuration = 1f},
                    
            new CameraStep {x = 7f, y = -14.1f, size = 3.5f,
                    LerpDuration = 1.5f, WaitDuration = 1f},
            
            new CameraStep {x = 7f, y = -12.1f, size = 1.8f,
                    LerpDuration = 1f, WaitDuration = 0f},
        };
    }
}

