using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFiniteStateMachineAnimator
{
    [RequireComponent(typeof(Animator))]
    public class MenuButtonCtrlBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected Image _imgBg;
        [SerializeField] protected Image _imgSource;
    
        private readonly int _enabled = Animator.StringToHash("Enabled");
        private readonly int _disabled = Animator.StringToHash("Disabled");
        private readonly int _enter = Animator.StringToHash("Enter");
        private readonly int _down = Animator.StringToHash("Down");
        private readonly int _up = Animator.StringToHash("Up");
        private readonly int _exit = Animator.StringToHash("Exit");
        
        public void SetStateEnabled()
        {
            _animator.SetTrigger(_enabled);
        }
        public void SetStateDisabled()
        {
            _animator.SetTrigger(_disabled);
        }
    
        #region UnityAnimEvent

        protected virtual void OnEnabled()
        {
            ResetAllTrigger();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        protected virtual void OnDisabled()
        {
            _imgBg.raycastTarget = false;
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public virtual void OnNormal()
        {
            _imgBg.raycastTarget = true;
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public virtual void OnHover()
        {
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public virtual void OnPressed()
        {
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public virtual void OnSelected()
        {
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        private void ResetAllTrigger()
        {
            _animator.ResetTrigger(_exit);
            _animator.ResetTrigger(_up);
            _animator.ResetTrigger(_down);
            _animator.ResetTrigger(_enabled);
            _animator.ResetTrigger(_disabled);
        }
        #endregion
    
        #region UnityPointerEvent
        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.SetTrigger("Enter");
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetTrigger("Exit");
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
            _animator.SetTrigger("Down");
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            _animator.SetTrigger("Up");
        }
        #endregion
    }
}