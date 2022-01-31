using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class FSMSpriteChanger : MonoBehaviour
    {
        public FSMUIBehaviour fsm;
        public Image image;
        public bool enableColorChange;
        [System.Serializable]
        public class Element
        {
            public Sprite sprite;
            [MyBox.ConditionalField(nameof(enableColorChange))]
            public Color color = new Color(1f, 1f, 1f, 1f);
        }
        public Element normal;
        public Element hover;
        public Element pressed;
        public Element selected;
        public Element dimmed;

        private void OnValidate()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            image = GetComponent<Image>();
            fsm.Register(HandleInput);
        }

        private void OnDisable()
        {
            image = GetComponent<Image>();
            fsm.Unregister(HandleInput);
        }

        private void HandleInput(FSMUIBehaviour.State state)
        {
            switch (state)
            {
                case FSMUIBehaviour.State.Normal:
                    UpdateSprite(normal);
                    break;
                case FSMUIBehaviour.State.Hover:
                    UpdateSprite(hover);
                    break;
                case FSMUIBehaviour.State.Pressed:
                    UpdateSprite(pressed);
                    break;
                case FSMUIBehaviour.State.Dimmed:
                    UpdateSprite(dimmed);
                    break;
                case FSMUIBehaviour.State.Selected:
                    UpdateSprite(selected);
                    break;
            }
        }

        private void UpdateSprite(Element element)
        {
            if (image != null)
            {
                if (element.sprite != null)
                {
                    image.sprite = element.sprite;
                }
                if (enableColorChange)
                {
                    image.color = element.color;
                }
            }
        }
    }
}