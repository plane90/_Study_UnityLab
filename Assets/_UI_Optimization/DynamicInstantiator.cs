using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInstantiator : MonoBehaviour
{
    public int size = 100;
    public GameObject target;
    void OnEnable()
    {
        for (int i = 0; i < size; i++)
        {
            var newGameObject = GameObject.Instantiate(target);
            newGameObject.SetActive(true);
        }
    }
}
