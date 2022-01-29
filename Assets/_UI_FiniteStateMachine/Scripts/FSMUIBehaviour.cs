using UnityEngine;

namespace UIFiniteStateMachine
{
    public class FSMUIBehaviour : MonoBehaviour
    {
        public enum State { Normal, Hover, Pressed, Dimmed, Selected, }
        [MyBox.ReadOnly] public State state;
    }
}