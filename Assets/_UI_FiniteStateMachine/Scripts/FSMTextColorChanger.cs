using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class FSMTextColorChanger : MonoBehaviour
    {
        public FSMUIBehaviour fsm;
        public Text text;
        [System.Serializable]
        public class Element
        {
            public bool isEnable;
            [MyBox.ConditionalField(nameof(isEnable))]
            public Color color = new Color(1f, 1f, 1f, 1f);
        }
        public Element normal;
        public Element hover;
        public Element pressed;
        public Element selected;
        public Element dimmed;

        private void OnValidate()
        {
            text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            text = GetComponent<Text>();
            fsm.Register(HandleInput);
            HandleInput(fsm.state);
        }

        private void OnDisable()
        {
            fsm.Unregister(HandleInput);
        }

        private void HandleInput(FSMUIBehaviour.State state)
        {
            switch (state)
            {
                case FSMUIBehaviour.State.Normal:
                    UpdateColor(normal);
                    break;
                case FSMUIBehaviour.State.Hover:
                    UpdateColor(hover);
                    break;
                case FSMUIBehaviour.State.Pressed:
                    UpdateColor(pressed);
                    break;
                case FSMUIBehaviour.State.Dimmed:
                    UpdateColor(dimmed);
                    break;
                case FSMUIBehaviour.State.Selected:
                    UpdateColor(selected);
                    break;
            }
        }

        private void UpdateColor(Element e)
        {
            if (text != null)
            {
                if (e.isEnable)
                {
                    text.color = e.color;
                }
            }
        }
    }
}