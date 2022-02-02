using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIFiniteStateMachine
{
    public class FSMRadioButtonGroup : MonoBehaviour
    {
        private event Action<int> onNotify;
        private List<FSMRadioButton> radioButtons = new List<FSMRadioButton>();

        private void Start()
        {
            foreach (var radioButton in radioButtons)
            {
                radioButton.IsOn = false;
            }
            radioButtons.OrderBy(e => e.transform.GetSiblingIndex()).First().IsOn = true;
        }

        public void Notify(int id)
        {
            onNotify?.Invoke(id);
        }

        public void Register(FSMRadioButton radioButton)
        {
            if (!radioButtons.Contains(radioButton))
            {
                radioButtons.Add(radioButton);
                onNotify += radioButton.OnClickedTo;
            }
        }

        public void Unregister(FSMRadioButton radioButton)
        {
            if (radioButtons.Contains(radioButton))
            {
                radioButtons.Remove(radioButton);
                onNotify -= radioButton.OnClickedTo;
            }
        }
    }
}