using System;
using UnityEngine;

namespace UIFiniteStateMachine
{
    public class FSMUIBehaviour : MonoBehaviour
    {
        public enum State { Normal, Hover, Pressed, Dimmed, Selected, }
        [MyBox.ReadOnly] public State state;
        public event Action<State> broadcastHandler;

        public void Register(Action<State> subscriber)
        {
            broadcastHandler += subscriber;
        }

        public void Unregister(Action<State> subscriber)
        {
            broadcastHandler += subscriber;
        }

        public void Broadcast(State state)
        {
            broadcastHandler?.Invoke(state);
        }
    }
}