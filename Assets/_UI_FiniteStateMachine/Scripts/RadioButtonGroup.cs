using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIFiniteStateMachine
{
    public class RadioButtonGroup : MonoBehaviour
    {
        public event Action<int> radioButtonHandler;
        public bool autoInit = true;
        [MyBox.ConditionalField(nameof(autoInit))]
        public RadioButton targetToInit;

        private List<RadioButton> radioButtons = new List<RadioButton>();

        private void Start()
        {
            if (!autoInit)
            {
                return;
            }

            foreach (var radioButton in radioButtons)
            {
                radioButton.Init(false);
            }

            // 지정된 라디오 버튼 On
            if (targetToInit != null)
            {
                targetToInit.Init(true);
                return;
            }

            // 하이어라키 상 가장 낮은 인덱스의 라디오 버튼 On
            var firstRadioButton = radioButtons.
                OrderBy(e => e.transform.GetSiblingIndex()).
                FirstOrDefault();
            if (firstRadioButton != null)
            {
                firstRadioButton.Init(true);
            }
        }

        public void Init(int id)
        {
            foreach (var radioButton in radioButtons)
            {
                radioButton.Init(false);
            }
            var target = radioButtons.Where(e => e.GetInstanceID() == id).FirstOrDefault();
            if (target != null)
            {
                target.Init(true);
            }
        }

        public void Notify(int id)
        {
            radioButtonHandler(id);
        }

        public void Register(RadioButton radioButton)
        {
            if (!radioButtons.Contains(radioButton))
            {
                radioButtons.Add(radioButton);
                radioButtonHandler += radioButton.OnClickedTo;
            }
        }

        public void Unregister(RadioButton radioButton)
        {
            if (radioButtons.Contains(radioButton))
            {
                radioButtons.Remove(radioButton);
                radioButtonHandler -= radioButton.OnClickedTo;
            }
        }
    }
}