using System;
using UnityEngine;

namespace AssetBookmark
{
    public class TargetScript : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log(GetInstanceID());
        }
    }
}