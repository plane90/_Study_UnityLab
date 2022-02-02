using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIFiniteStateMachine
{
    public class FSMToggleGroup : MonoBehaviour
    {
        private event Action<int> onNotify;
        private List<FSMToggle> toggles = new List<FSMToggle>();

        private void Start()
        {
            foreach (var radioButton in toggles)
            {
                radioButton.IsOn = false;
            }
            toggles.OrderBy(e => e.transform.GetSiblingIndex()).First().IsOn = true;
        }

        public void Notify(int id)
        {
            onNotify?.Invoke(id);
        }

        public void Register(FSMToggle radioButton)
        {
            if (!toggles.Contains(radioButton))
            {
                toggles.Add(radioButton);
                //onNotify += radioButton.OnClickedTo;
            }
        }

        public void Unregister(FSMToggle radioButton)
        {
            if (toggles.Contains(radioButton))
            {
                toggles.Remove(radioButton);
                //onNotify -= radioButton.OnClickedTo;
            }
        }
    }
}