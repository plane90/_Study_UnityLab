using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIFiniteStateMachine
{
    public class FSMRadioButtonGroup : MonoBehaviour
    {
        private event Action<int> broadcastHandler;
        private List<FSMRadioButton> radioButtons = new List<FSMRadioButton>();

        private void Start()
        {
            foreach (var radioButton in radioButtons)
            {
                radioButton.isOn = false;
            }
            radioButtons.OrderBy(e => e.transform.GetSiblingIndex()).First().isOn = true;
        }

        public void Notify(int id)
        {
            broadcastHandler?.Invoke(id);
        }

        public void Register(FSMRadioButton radioButton)
        {
            if (!radioButtons.Contains(radioButton))
            {
                radioButtons.Add(radioButton);
                broadcastHandler += radioButton.OnClickedTo;
            }
        }

        public void Unregister(FSMRadioButton radioButton)
        {
            if (radioButtons.Contains(radioButton))
            {
                radioButtons.Remove(radioButton);
                broadcastHandler -= radioButton.OnClickedTo;
            }
        }
    }
}