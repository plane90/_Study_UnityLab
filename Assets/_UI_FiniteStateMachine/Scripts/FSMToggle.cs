using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIFiniteStateMachine
{
    public class FSMToggle : FSMUIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool interactable = true;
        [SerializeField] private bool isOn = true;

        private enum Input { Enabled, Disabled, Enter, NorExit, SelExit, Down, Deselected, Selected, }
        private bool curInteractable = true;
        private bool curIsOn = true;

        [System.Serializable] public class UnityEventBool : UnityEvent<bool> { };
        public UnityEventBool onValueChanged;
        public UnityEvent onSelected;
        public UnityEvent onDeselected;
        public UnityEvent onDimmed;

        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
                if (CheckInteractableChange())
                {
                    curInteractable = Interactable;
                    if (Interactable)
                    {
                        HandleInput(Input.Enabled);
                    }
                    else
                    {
                        HandleInput(Input.Disabled);
                        onDimmed?.Invoke();
                    }
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
                if (CheckIsOnChange())
                {
                    curIsOn = IsOn;
                    onValueChanged?.Invoke(IsOn);
                }
                if (IsOn)
                {
                    onSelected?.Invoke();
                    HandleInput(Input.Selected);
                }
                else
                {
                    onDeselected?.Invoke();
                    HandleInput(Input.Deselected);
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
            curInteractable = Interactable;
            curIsOn = IsOn;
        }

        private bool CheckInteractableChange()
        {
            if (!curInteractable.Equals(Interactable))
            {
                return true;
            }
            return false;
        }
        
        private bool CheckIsOnChange()
        {
            if (!curIsOn.Equals(IsOn))
            {
                return true;
            }
            return false;
        }

        private void HandleInput(Input input)
        {
            switch (state)
            {
                case State.Normal:
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    else if (input.Equals(Input.Enter))
                    {
                        state = State.Hover;
                    }
                    break;
                case State.Hover:
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    else if (input.Equals(Input.Down))
                    {
                        state = State.Pressed;
                    }
                    else if (input.Equals(Input.NorExit))
                    {
                        state = State.Normal;
                    }
                    else if (input.Equals(Input.SelExit))
                    {
                        state = State.Selected;
                    }
                    break;
                case State.Pressed:
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    else if (input.Equals(Input.NorExit))
                    {
                        state = State.Normal;
                    }
                    else if (input.Equals(Input.SelExit))
                    {
                        state = State.Selected;
                    }
                    else if (input.Equals(Input.Selected))
                    {
                        state = State.Selected;
                    }
                    else if (input.Equals(Input.Deselected))
                    {
                        state = State.Hover;
                    }
                    break;
                case State.Selected:
                    if (input.Equals(Input.Disabled))
                    {
                        state = State.Dimmed;
                    }
                    else if (input.Equals(Input.Enter))
                    {
                        state = State.Hover;
                    }
                    else if (input.Equals(Input.Down))
                    {
                        state = State.Pressed;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            IsOn = !IsOn;
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
            HandleInput(IsOn ? Input.SelExit : Input.NorExit);
        }
    }
}