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
        [System.Serializable] public class UnityEventBool : UnityEvent<bool> { };
        public UnityEventBool onValueChanged;
        public UnityEvent onSelected;
        public UnityEvent onDeselected;
        public UnityEvent onDimmed;

        private enum Input { Enabled, Disabled, Enter, Exit, Down, NonSelected, Selected, Deselected, }
        private bool curInteractable;
        private bool curIsOn = false;
        [SerializeField] private bool interactable = true;
        [SerializeField] private bool isOn = false;

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

        public bool IsOn
        {
            get => isOn;
            set
            {
                if (!Interactable)
                {
                    return;
                }
                isOn = value;
                if (IsOn)
                {
                    onSelected?.Invoke();
                    HandleInput(Input.Selected);
                    if (CheckIsOnChange())
                    {
                        curIsOn = IsOn;
                        onValueChanged?.Invoke(curIsOn);
                    }
                }
                else
                {
                    onDeselected?.Invoke();
                    if (CheckIsOnChange())
                    {
                        curIsOn = IsOn;
                        onValueChanged?.Invoke(curIsOn);
                        HandleInput(Input.Deselected);
                    }
                    else
                    {
                        HandleInput(Input.NonSelected);
                    }
                }
            }
        }

        [MyBox.ButtonMethod]
        public void ToggleInteractable()
        {
            Interactable = !Interactable;
        }
        [MyBox.ButtonMethod]
        public void ToggleIsOn()
        {
            IsOn = !IsOn;
        }

        private void OnEnable()
        {
            group.Register(this);
            curIsOn = IsOn;

            curInteractable = Interactable;
            if (!curInteractable)
            {
                HandleInput(Input.Disabled);
                return;
            }
            if (IsOn)
            {
                HandleInput(Input.Selected);
            }
            else
            {
                HandleInput(Input.Deselected);
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
            Broadcast(state);
        }

        public void OnClickedTo(int id)
        {
            if (!curInteractable)
            {
                return;
            }
            if (id.Equals(GetInstanceID()))
            {
                IsOn = true;
            }
            else
            {
                IsOn = false;
            }
        }

        private bool CheckInteractableChange()
        {
            if (curInteractable != Interactable)
            {
                return true;
            }
            return false;
        }
        private bool CheckIsOnChange()
        {
            if (curIsOn != IsOn)
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