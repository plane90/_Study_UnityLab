using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class Basic_FSMMouseInputButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Button target;
        public enum State { Normal, Hover, Pressed, Dimmed, }
        public State state = State.Normal;

        private enum MouseInput { Enabled, Disabled, Enter, Exit, Down, Click, }
        private bool curInteractable;

        private void OnEnable()
        {
            curInteractable = target.interactable;
            if (!curInteractable)
            {
                HandleInput(MouseInput.Disabled);
                return;
            }
            HandleInput(MouseInput.Enabled);
        }

        private void Update()
        {
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
                    if (input.Equals(MouseInput.Click))
                    {
                        state = State.Hover;
                    }
                    if (input.Equals(MouseInput.Exit))
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

        public void OnPointerClick(PointerEventData eventData)
        {
            HandleInput(MouseInput.Click);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleInput(MouseInput.Down);
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