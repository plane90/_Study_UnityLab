using UnityEngine;

namespace UIFiniteStateMachineAnimator
{
    public class MainMenuButton : MenuButtonCtrlBase
    {
        protected override void OnEnabled()
        {
            base.OnEnabled();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public override void OnNormal()
        {
            base.OnNormal();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public override void OnHover()
        {
            base.OnHover();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }

        public override void OnSelected()
        {
            base.OnSelected();
            Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod()}");
        }
    }
}