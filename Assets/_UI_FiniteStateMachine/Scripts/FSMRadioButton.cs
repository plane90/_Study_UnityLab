using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIFiniteStateMachine
{
    public class FSMRadioButton : FSMUIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public FSMRadioButtonGroup group;
        public bool interactable = true;
        public bool isOn = false;
        [System.Serializable] public class UnityEventBool : UnityEvent<bool> { };
        public UnityEventBool onValueChanged;
        public UnityEvent onSelected;
        public UnityEvent onDeselected;
        public UnityEvent onDimmed;

        private enum Input { Enabled, Disabled, Enter, Exit, Down, NonSelected, Selected, Deselected, }
        private bool curInteractable;
        private bool curIsOn = false;

        private void OnEnable()
        {
            group.Register(this);
            curIsOn = isOn;

            curInteractable = interactable;
            if (!curInteractable)
            {
                HandleInput(Input.Disabled);
                return;
            }
            if (isOn)
            {
                HandleInput(Input.Selected);
            }
            else
            {
                HandleInput(Input.Deselected);
            }
        }

        private void Update()
        {
            if (CheckInteractableChange())
            {
                curInteractable = interactable;
                if (!curInteractable)
                {
                    onDimmed.Invoke();
                }
                var input = curInteractable ? Input.Enabled : Input.Disabled;
                HandleInput(input);
            }
            if (CheckIsOnChange())
            {
                group.Notify(GetInstanceID());
            }
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
                    if (input.Equals(Input.Selected))
                    {
                        state = State.Selected;
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
                    if (input.Equals(Input.Exit))
                    {
                        state = State.Normal;
                    }
                    if (input.Equals(Input.Selected))
                    {
                        state = State.Selected;
                    }
                    if (input.Equals(Input.NonSelected))
                    {
                        state = State.Hover;
                    }
                    break;
                case State.Selected:
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    if (input.Equals(Input.Deselected))
                    {
                        state = State.Normal;
                    }
                    break;
                case State.Dimmed:
                    if (input.Equals(Input.Enabled))
                    {
                        state = State.Normal;
                    }
                    break;
            }
        }

        public void OnClickedTo(int id)
        {
            if (!curInteractable)
            {
                return;
            }
            if (id.Equals(GetInstanceID()))
            {
                isOn = true;
                onSelected?.Invoke();
                if (CheckIsOnChange())
                {
                    curIsOn = isOn;
                    onValueChanged?.Invoke(curIsOn);
                }
                HandleInput(Input.Selected);
            }
            else
            {
                isOn = false;
                onDeselected?.Invoke();
                if (CheckIsOnChange())
                {
                    curIsOn = isOn;
                    onValueChanged?.Invoke(curIsOn);
                    HandleInput(Input.Deselected);
                }
                else
                {
                    HandleInput(Input.NonSelected);
                }
            }
        }

        private bool CheckInteractableChange()
        {
            if (curInteractable != interactable)
            {
                return true;
            }
            return false;
        }
        private bool CheckIsOnChange()
        {
            if (curIsOn != isOn)
            {
                return true;
            }
            return false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!curInteractable)
            {
                return;
            }
            group.Notify(GetInstanceID());
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