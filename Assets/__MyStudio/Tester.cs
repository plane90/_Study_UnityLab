using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class Tester : MonoBehaviour
{
    public Button button;
    public Tester tester;

    [MyScene] public string myScene;

    public string str = "abc";
    private void Start()
    {
        tester ??= GetComponent<Tester>();
        Debug.Log("Start");
        Debug.Log("End");
    }

    [MyBox.ButtonMethod()]
    public void Test()
    {
        SceneManager.LoadSceneAsync(myScene, LoadSceneMode.Additive);
    }
}