namespace UIFiniteStateMachineAnimator
{
    public interface ISomeAnimationEventHandler
    {
        public void OnEnabledState();
        public void OnDisabledState();
        public void OnNormalState();
        public void OnHoverState();
        public void OnPressedState();
        public void OnSelectedState();
        public void OnReadyState();
    }
}