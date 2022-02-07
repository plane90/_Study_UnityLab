using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Find
{
    public class ReferenceDeactivatedObject : MonoBehaviour
    {
        public Finder deactivatedItem;
        private void OnEnable()
        {
            if (deactivatedItem == null)
            {
                Debug.Log("null");
            }
        }
    }
}