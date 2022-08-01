using System;
using UnityEngine;

namespace SceneReference
{
    [Serializable]
    public class ScenePath
    {
        [SerializeField] private string _scenePath = "empty";

        public override string ToString()
        {
            return _scenePath;
        }
    }
}