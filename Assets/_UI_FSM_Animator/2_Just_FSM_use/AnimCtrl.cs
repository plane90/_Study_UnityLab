using UnityEngine;
using UnityEngine.EventSystems;

namespace UIFiniteStateMachineAnimator
{
    [RequireComponent(typeof(Animator))]
    public class AnimCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] private Animator _anim;

        private ISomeAnimationEventHandler _eventHandler;

        #region UnityAnimatorEvent
        public void OnEnabled()
        {
            _eventHandler.OnEnabledState();
        }

        public void OnDisabled()
        {
            _eventHandler.OnDisabledState();
        }

        public void OnReady()
        {
            _eventHandler.OnReadyState();
        }

        public void OnNormal()
        {
            _eventHandler.OnNormalState();
        }
        
        public void OnHover()
        {
            _eventHandler.OnHoverState();
        }
        
        public void OnPressed()
        {
            _eventHandler.OnPressedState();
        }

        public void OnSelected()
        {
            _eventHandler.OnSelectedState();
        }
        #endregion

        #region UnityPointerEvent
        public void OnPointerEnter(PointerEventData eventData)
        {
            _anim.SetTrigger("Enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _anim.SetTrigger("Exit");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _anim.SetTrigger("Down");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _anim.SetTrigger("Up");
        }

        public void Register(ISomeAnimationEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }
        #endregion
    }
}