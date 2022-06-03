using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleThreadCheck : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Foo());
        int cnt = 100000;
        do
        {
            Debug.Log($"{cnt}");
            cnt--;
        } while (cnt > 0);
    }

    private IEnumerator Foo()
    {
        var startTime = Time.time;
        Debug.Log($"Coroutine Started at {Time.time}");
        yield return new WaitForSeconds(1f);
        var endTime = Time.time;
        Debug.Log($"Coroutine ended at {endTime} / elapsed time: {endTime - startTime}");
    }
}
