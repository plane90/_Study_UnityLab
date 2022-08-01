using System;
using System.Collections;
using System.Collections.Generic;
using Loading;
using MyBox;
using SceneReference;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Tester_EditorMode : MonoBehaviour
{
    [SerializeField] private ScenePath sceneRef;
    [SerializeField] private List<ScenePath> _mySceneReferences;
    [SerializeField] private GameObject _go;

    private void OnEnable()
    {
        _mySceneReferences.ForEach(Debug.Log);
    }

    [ButtonMethod()]
    public void LoadTest()
    {
        SceneLoader.Instance?.LoadScene(new [] { sceneRef }, null, true);
    }

    [ButtonMethod()]
    public void LoadsTest()
    {
        SceneLoader.Instance?.LoadScene(_mySceneReferences, null, true);
    }
}