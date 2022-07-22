using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIFiniteStateMachineAnimator
{
    public class AnimCtrlClient_A : MonoBehaviour, ISomeAnimationEventHandler
    {
        [SerializeField] private AnimCtrl animCtrl;
        [SerializeField] private Image _imgSource;

        private void Start()
        {
            animCtrl.Register(this);
        }

        #region SomeAnimationEventHandler

        public void OnReadyState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
            _imgSource.DOColor(new Color(1f, 1f, 1f, 0f), 0);
        }

        public void OnEnabledState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
            _imgSource.DOColor(new Color(1f, 1f, 1f, 1f), 0.2f);
            //DOTween.To((() => _imgSource.color), x => _imgSource.color = x, new Color(1f, 1f, 1f, 0f), 1);
        }

        public void OnDisabledState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
            _imgSource.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f);
        }

        public void OnNormalState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
            _imgSource.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f);
        }

        public void OnHoverState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
        }

        public void OnPressedState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
        }

        public void OnSelectedState()
        {
            Debug.Log($"{System.Reflection.MethodInfo.GetCurrentMethod()}!");
        }

        #endregion
    }
}