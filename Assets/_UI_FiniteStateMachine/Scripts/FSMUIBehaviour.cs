using System;
using UnityEngine;

namespace UIFiniteStateMachine
{
    public class FSMUIBehaviour : MonoBehaviour
    {
        public enum State { Normal, Hover, Pressed, Selected, Dimmed, }
        [MyBox.ReadOnly] public State state;
        public event Action<State> onStateChanged;
        public event Action<State, bool> onStateChangedWithSelected;

        public void Register(Action<State> listener)
        {
            onStateChanged += listener;
        }

        public void Unregister(Action<State> listener)
        {
            onStateChanged += listener;
        }

        public void Register(Action<State, bool> listener)
        {
            onStateChangedWithSelected += listener;
        }

        public void Unregister(Action<State, bool> listener)
        {
            onStateChangedWithSelected += listener;
        }

        public void Broadcast(State state)
        {
            Debug.Log($"FSMUIBehaviour: {GetInstanceID()}");
            onStateChanged?.Invoke(state);
        }

        public void Broadcast(State state, bool isSelected)
        {
            Debug.Log($"FSMUIBehaviour: {GetInstanceID()}");
            onStateChangedWithSelected?.Invoke(state, isSelected);
        }
    }
}