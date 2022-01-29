using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class Basic_FSMMouseInputRadio : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Toggle target;
        public enum State { Normal, Hover, Pressed, Dimmed, Selected, }
        public State state = State.Normal;

        private enum MouseInput { Enabled, Disabled, Enter, Exit, Down, Deselected, Selected, }
        private bool curInteractable;

        private void OnEnable()
        {
            curInteractable = target.interactable;
            if (!curInteractable)
            {
                HandleInput(MouseInput.Disabled);
                return;
            }
            if (target.isOn)
            {
                HandleInput(MouseInput.Selected);
            }
        }

        private void Update()
        {
            Debug.Log($"{gameObject.name} State:{state}");
            if (CheckInteractableChange())
            {
                var input = curInteractable ? MouseInput.Enabled : MouseInput.Disabled;
                HandleInput(input);
            }
        }

        private void HandleInput(MouseInput input)
        {
            switch (state)
            {
                case State.Normal:
                    if (input.Equals(MouseInput.Enter))
                    {
                        state = State.Hover;
                    }
                    if (input.Equals(MouseInput.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    if (input.Equals(MouseInput.Selected))
                    {
                        state = State.Selected;
                    }
                    break;
                case State.Hover:
                    if (input.Equals(MouseInput.Exit))
                    {
                        state = State.Normal;
                    }
                    else if (input.Equals(MouseInput.Down))
                    {
                        state = State.Pressed;
                    }
                    else if (input.Equals(MouseInput.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    break;
                case State.Pressed:
                    if (input.Equals(MouseInput.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    if (input.Equals(MouseInput.Selected))
                    {
                        state = State.Selected;
                    }
                    if (input.Equals(MouseInput.Exit))
                    {
                        state = State.Normal;
                    }
                    break;
                case State.Selected:
                    if (input.Equals(MouseInput.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    if (input.Equals(MouseInput.Deselected))
                    {
                        state = State.Normal;
                    }
                    break;
                case State.Dimmed:
                    if (input.Equals(MouseInput.Enabled))
                    {
                        state = State.Normal;
                    }
                    break;
            }
        }

        private bool CheckInteractableChange()
        {
            if (curInteractable != target.interactable)
            {
                curInteractable = target.interactable;
                return true;
            }
            return false;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            HandleInput(MouseInput.Down);
        }

        public void OnValueChanged(bool isSelected)
        {
            var input = isSelected ? MouseInput.Selected : MouseInput.Deselected;
            HandleInput(input);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HandleInput(MouseInput.Enter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HandleInput(MouseInput.Exit);
        }
    }
}