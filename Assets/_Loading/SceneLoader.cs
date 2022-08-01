using System.Collections;
using System.Collections.Generic;
using SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;

        public static SceneLoader Instance
        {
            get
            {
                _instance ??= FindObjectOfType<SceneLoader>();
                _instance ??= new GameObject().AddComponent<SceneLoader>();
                DontDestroyOnLoad(_instance);
                return _instance;
            }
        }

        public void LoadScene(IEnumerable<ScenePath> scenePaths, Sprite loadingContext, bool showLoadingDisplay)
        {
            StartCoroutine(LoadSceneAsync(scenePaths, loadingContext, showLoadingDisplay));
        }

        private IEnumerator LoadSceneAsync(IEnumerable<ScenePath> scenePaths, Sprite loadingContext,
            bool showLoadingDisplay)
        {
            LoadingDisplay ld = null;
            if (showLoadingDisplay)
            {
                ld = (Instantiate(Resources.Load("_Loading/LoadingDisplay")) as GameObject)
                    ?.GetComponent<LoadingDisplay>();
                ld.SetContext(loadingContext);
                ld.Show();
                DontDestroyOnLoad(ld);
            }

            var e = scenePaths.GetEnumerator();
            e.MoveNext();
            yield return StartCoroutine(ProceedLoad(e.Current, LoadSceneMode.Single));

            while (e.MoveNext())
            {
                yield return StartCoroutine(ProceedLoad(e.Current, LoadSceneMode.Additive));
            }

            if (showLoadingDisplay)
            {
                ld?.Hide(1f);
            }

            Destroy(gameObject, 1f);
        }

        private IEnumerator ProceedLoad(ScenePath path, LoadSceneMode mode)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(path, mode);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress > 0.9)
            {
                yield return null;
            }

            asyncOperation.allowSceneActivation = true;
        }
    }
}