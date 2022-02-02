using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class FSMSpriteChangerEx : MonoBehaviour
    {
        public FSMUIBehaviour fsm;
        public Image image;

        [System.Serializable]
        public class SpriteAndColor
        {
            public Sprite sprite;
            public Color color = new Color(1f, 1f, 1f, 1f);
        }

        [System.Serializable]
        public class SpriteChangerElement
        {
            public bool enableColorChange;
            public SpriteAndColor normal;
            public SpriteAndColor hover;
            public SpriteAndColor pressed;
            public SpriteAndColor selected;
            public SpriteAndColor dimmed;
        }
        public SpriteChangerElement Sprites;
        public bool enableSelectedSprites;
        [MyBox.ConditionalField(nameof(enableSelectedSprites))]
        public SpriteChangerElement selectedSprites;


        private void OnValidate()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            image = GetComponent<Image>();
            //if (enableSelectedSprites)
            //{
            //    fsm.Register(HandleInputWithIsSelected);
            //}
            //else
            //{
            //    fsm.Register(HandleInput);
            //}
        }

        private void OnDisable()
        {
            image = GetComponent<Image>();
            //if (enableSelectedSprites)
            //{
            //    fsm.Unregister(HandleInputWithIsSelected);
            //}
            //else
            //{
            //    fsm.Unregister(HandleInput);
            //}
        }

        private void LateUpdate()
        {
            var toggle = fsm as FSMToggleEx;
            if (toggle != null)
            {
                HandleInputWithIsSelected(toggle.state, toggle.IsOn);
            }
            else
            {
                HandleInput(fsm.state);
            }
        }

        private void HandleInput(FSMUIBehaviour.State state)
        {
            Debug.Log($"{gameObject.name}: HandleInput");
            switch (state)
            {
                case FSMUIBehaviour.State.Normal:
                    UpdateSprite(Sprites.normal, Sprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Hover:
                    UpdateSprite(Sprites.hover, Sprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Pressed:
                    UpdateSprite(Sprites.pressed, Sprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Dimmed:
                    UpdateSprite(Sprites.dimmed, Sprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Selected:
                    UpdateSprite(Sprites.selected, Sprites.enableColorChange);
                    break;
            }
        }

        private void HandleInputWithIsSelected(FSMUIBehaviour.State state, bool isSelected)
        {
            Debug.Log($"{gameObject.name}: HandleInputWithIsSelected");
            if (!isSelected)
            {
                HandleInput(state);
                return;
            }
            switch (state)
            {
                case FSMUIBehaviour.State.Normal:
                    UpdateSprite(selectedSprites.normal, selectedSprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Hover:
                    UpdateSprite(selectedSprites.hover, selectedSprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Pressed:
                    UpdateSprite(selectedSprites.pressed, selectedSprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Dimmed:
                    UpdateSprite(selectedSprites.dimmed, selectedSprites.enableColorChange);
                    break;
                case FSMUIBehaviour.State.Selected:
                    UpdateSprite(selectedSprites.selected, selectedSprites.enableColorChange);
                    break;
            }
        }

        private void UpdateSprite(SpriteAndColor element, bool enableColorChange)
        {
            if (image != null)
            {
                image.sprite = element.sprite;
                if (enableColorChange)
                {
                    image.color = element.color;
                }
                else
                {
                    image.color = Color.white;
                }
            }
        }
    }
}