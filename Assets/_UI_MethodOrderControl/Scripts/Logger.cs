using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIMethodOrderControl
{
    public class Logger : MonoBehaviour
    {
        public int id;
        private void OnEnable()
        {
            Debug.Log($"id:{id} OnEnable");
        }
        private void Start()
        {
            Debug.Log($"id:{id} Start");
        }
    }
}