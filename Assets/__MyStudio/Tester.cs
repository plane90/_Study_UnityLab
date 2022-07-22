using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private IEnumerator Start()
    {
        Debug.Log("Start");
        yield return StartCoroutine(Coroutine());
        Debug.Log("End");
    }

    public IEnumerator Coroutine()
    {
        var elapsed = 0f;
        while (true)
        {
            if (elapsed > 5f)
            {
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
