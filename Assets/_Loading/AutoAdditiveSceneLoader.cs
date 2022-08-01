using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SceneReference;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loading
{
    [ExecuteInEditMode]
    public class AutoAdditiveSceneLoader : MonoBehaviour
    {
        [SerializeField] private List<ScenePath> _scenePaths;

        private void Start()
        {
#if UNITY_EDITOR
            StartCoroutine(AdditiveLoadSceneInEditor());
#else
            StartCoroutine(AdditiveLoadSceneInRuntime());
#endif
        }

        private IEnumerator AdditiveLoadSceneInEditor()
        {
            if (Application.isPlaying) yield break;

            foreach (var scenePath in _scenePaths.Where(x => !string.IsNullOrEmpty(x)))
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }

        private IEnumerator AdditiveLoadSceneInRuntime()
        {
            if (!Application.isPlaying) yield break;

            foreach (var scenePath in _scenePaths.Where(x => !string.IsNullOrEmpty(x)))
            {
                yield return StartCoroutine(LoadAsyncScene(scenePath));
            }

            Debug.Log("All Scene Loaded");
        }

        private IEnumerator LoadAsyncScene(ScenePath scenePath)
        {
            Debug.Log("Scene Load Start : " + scenePath);
            var asyncOperation = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }
            asyncOperation.allowSceneActivation = true;
        }
    }
}