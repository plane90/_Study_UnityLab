using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class ToggleIsOnSetter : MonoBehaviour
    {
        public Toggle target;
        private void OnEnable()
        {
            target.isOn = false;
        }

        public void OnValueChanged(bool value)
        {
            Debug.Log($"{value}");
        }
    }
}