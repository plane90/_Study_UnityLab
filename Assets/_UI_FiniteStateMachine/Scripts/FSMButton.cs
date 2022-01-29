using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIFiniteStateMachine
{
    public class FSMButton : FSMUIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent onClicked;
        public UnityEvent onEnabled;
        public UnityEvent onDimmed;

        private enum Input { Enabled, Disabled, Enter, Exit, Down, Click, }
        private bool curInteractable;
        [SerializeField] private bool interactable = true;

        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
                if (CheckInteractableChange())
                {
                    curInteractable = value;
                    if (!curInteractable)
                    {
                        onDimmed?.Invoke();
                    }
                    var input = curInteractable ? Input.Enabled : Input.Disabled;
                    HandleInput(input);
                }
            }
        }

        [MyBox.ButtonMethod]
        public void ToggleInteractable()
        {
            Interactable = !Interactable;
        }

        private void OnEnable()
        {
            curInteractable = interactable;
        }

        private void HandleInput(Input input)
        {
            switch (state)
            {
                case State.Normal:
                    if (input.Equals(Input.Enter))
                    {
                        state = State.Hover;
                    }
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    break;
                case State.Hover:
                    if (input.Equals(Input.Exit))
                    {
                        state = State.Normal;
                    }
                    else if (input.Equals(Input.Down))
                    {
                        state = State.Pressed;
                    }
                    else if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    break;
                case State.Pressed:
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    if (input.Equals(Input.Click))
                    {
                        onClicked?.Invoke();
                        state = State.Hover;
                    }
                    if (input.Equals(Input.Exit))
                    {
                        state = State.Normal;
                    }
                    break;
                case State.Dimmed:
                    if (input.Equals(Input.Enabled))
                    {
                        state = State.Normal;
                        onEnabled?.Invoke();
                    }
                    break;
            }
            Broadcast(state);
        }

        private bool CheckInteractableChange()
        {
            if (curInteractable != interactable)
            {
                return true;
            }
            return false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HandleInput(Input.Click);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleInput(Input.Down);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HandleInput(Input.Enter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HandleInput(Input.Exit);
        }
    }
}