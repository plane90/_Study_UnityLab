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
        private MouseInput input = MouseInput.Enabled;
        private bool curInteractable;

        private void OnEnable()
        {
            curInteractable = target.interactable;
            if (!curInteractable)
            {
                input = MouseInput.Disabled;
                return;
            }
            if (target.isOn)
            {
                input = MouseInput.Selected;
            }
        }

        private void Update()
        {
            Debug.Log($"{gameObject.name} State:{state}");
            if (CheckInteractableChange())
            {
                input = curInteractable ? MouseInput.Enabled : MouseInput.Disabled;
            }
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
            input = MouseInput.Down;
        }

        public void OnValueChanged(bool isSelected)
        {
            input = isSelected ? MouseInput.Selected : MouseInput.Deselected;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            input = MouseInput.Enter;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            input = MouseInput.Exit;
        }
    }
}