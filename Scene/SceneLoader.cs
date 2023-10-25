using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    #region singleton
    public static SceneLoader instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void OnEnable()
    {
        EventHelper.SceneLoaderEvent += LoadScene;
    }

    private void OnDisable()
    {
        EventHelper.SceneLoaderEvent -= LoadScene;
    }

    public void LoadScene(string sceneToLoad)
    {
        StartCoroutine(StartLoad(sceneToLoad));
    }

    IEnumerator StartLoad(string sceneToLoad)
    {
        canvasGroup.gameObject.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.25f));
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0, 0.25f));
        canvasGroup.gameObject.SetActive(false);
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}