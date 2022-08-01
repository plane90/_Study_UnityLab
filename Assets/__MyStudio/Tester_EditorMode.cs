using System;
using System.Collections;
using System.Collections.Generic;
using SceneReference;
using UnityEngine;

[ExecuteInEditMode]
public class Tester_EditorMode : MonoBehaviour
{
    [SerializeField] private ScenePath sceneRef;
    [SerializeField] private List<ScenePath> _mySceneReferences;

    private void OnEnable()
    {
        Debug.Log(sceneRef);
    }
}
