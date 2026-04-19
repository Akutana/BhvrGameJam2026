using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistentServices : MonoBehaviour
{
    public static PersistentServices Instance { get; private set; }

    
    [Header("UI")]
    public TextMeshProUGUI interactionText;

    [Header("Fade")]
    public RawImage fadeImage;
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource sfxSource; // for short one-off sounds like truck start, door creak etc.

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DoFade(1f, 0f));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionTo(sceneName));
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    IEnumerator TransitionTo(string sceneName)
    {
        yield return StartCoroutine(DoFade(0f, 1f)); // fade to black
        fadeImage.color = new Color(0, 0, 0, 1f);   // ensure it stays black
        SceneManager.LoadScene(sceneName);
        // OnSceneLoaded fires automatically and fades in
    }

    IEnumerator DoFade(float from, float to)
    {
        float elapsed = 0f;
        fadeImage.color = new Color(0, 0, 0, from);
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(from, to, elapsed / fadeDuration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, to);
    }
}