using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            //var a = GetComponentsInParent<Light>().FirstOrDefault();
            var a = GetComponentsInParent<Light>().Length;
            Debug.Log("null but no error!");

        }

        public void OnValueChanged(bool value)
        {
            Debug.Log($"{value}");
        }
    }
}