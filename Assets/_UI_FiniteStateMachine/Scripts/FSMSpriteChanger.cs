using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class FSMSpriteChanger : MonoBehaviour
    {
        public FSMRadioButton fsm;
        public Image image;
        public bool enableColorChange;
        [System.Serializable]
        public class SpriteAndColor
        {
            public Sprite sprite;
            public Color color = new Color(1f, 1f, 1f, 1f);
        }
        public SpriteAndColor normal;
        public SpriteAndColor hover;
        public SpriteAndColor pressed;
        public SpriteAndColor selected;
        public SpriteAndColor dimmed;

        private void OnValidate()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (image == null)
            {
                return;
            }
            switch (fsm.state)
            {
                case FSMRadioButton.State.Normal:
                    UpdateSprite(normal);
                    break;
                case FSMRadioButton.State.Hover:
                    UpdateSprite(hover);
                    break;
                case FSMRadioButton.State.Pressed:
                    UpdateSprite(pressed);
                    break;
                case FSMRadioButton.State.Dimmed:
                    UpdateSprite(dimmed);
                    break;
                case FSMRadioButton.State.Selected:
                    UpdateSprite(selected);
                    break;
                default:
                    break;
            }
        }

        private void UpdateSprite(SpriteAndColor element)
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
