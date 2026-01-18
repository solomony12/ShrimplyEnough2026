using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    public static SceneTransition Instance { get; private set; }

    public static bool IsTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        canvasGroup = fadeImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = false;
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void StartTransition(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        // Block clicks
        canvasGroup.blocksRaycasts = true;
        IsTransitioning = true;
        PlayerController.DisablePlayerControl();

        // Fade in
        yield return StartCoroutine(Fade(0f, 1f));

        // Load the new scene
        yield return SceneManager.LoadSceneAsync(sceneName);
        PlayerController.DisablePlayerControl();

        // Fade out
        yield return StartCoroutine(Fade(1f, 0f));

        // Allow clicks again
        canvasGroup.blocksRaycasts = false;
        IsTransitioning = false;
        PlayerController.EnablePlayerControl();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, endAlpha);
    }
}
