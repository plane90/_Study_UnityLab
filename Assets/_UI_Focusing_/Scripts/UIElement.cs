using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFocusing
{
    public class UIElement : MonoBehaviour
    {
        public string key;
        private void Start()
        {
            key = string.IsNullOrEmpty(key) ? gameObject.name : key;
            GetComponentInParent<UIElementGroup>().uiElementMap.Add(key, this);
        }
    }

}