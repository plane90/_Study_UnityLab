using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Find
{
    public class Finder : MonoBehaviour
    {
        public GameObject target;

        private void OnEnable()
        {
            Debug.Log($"target.name: {target.name}, {transform.parent.Find(target.name)}");
        }
    }
}