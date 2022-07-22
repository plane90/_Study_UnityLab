using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LobbyRoleToggleAnimCtrl : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _imgRaycasterTarget;
    [SerializeField] private List<Image> _imgAnimationTargets;

    private readonly int _enabled = Animator.StringToHash("Enabled");
    private readonly int _disabled = Animator.StringToHash("Disabled");
    private readonly int _enter = Animator.StringToHash("Enter");
    private readonly int _down = Animator.StringToHash("Down");
    private readonly int _up = Animator.StringToHash("Up");
    private readonly int _exit = Animator.StringToHash("Exit");
    private readonly int _dimmed = Animator.StringToHash("Dimmed");
    private readonly int _isSelected = Animator.StringToHash("IsSelected");

    public void Enable()
    {
        _animator.SetTrigger(_enabled);
    }
    
    #region UnityAnimEvent

    protected virtual void OnReadyState()
    {
        _imgAnimationTargets.ForEach(x => x.color = new Color(1f, 1f, 1f, 0f));
    }

    protected virtual void OnEnabledState()
    {
        ResetAllTrigger();
        _imgAnimationTargets.ForEach(x => x.DOColor(Color.white, 0.2f));
    }

    private IEnumerator FadeColor(bool isAppear)
    {
        var desiredAlpha = isAppear ? 1f : 0f;
        var speed = 1.0f;
        var target = _imgAnimationTargets.FirstOrDefault();
        if (target == null) yield break;
        while (Math.Abs(target.color.a - desiredAlpha) > 0.01f)
        {
            var amount = Time.deltaTime * (isAppear ? 1 : -1) * speed;
            var newAlpha = Mathf.Clamp(target.color.a + amount, 0f, 1f);
            var newColor = new Color(1f, 1f, 1f, newAlpha);
            _imgAnimationTargets.ForEach(x => x.color = newColor);
            yield return null;
        }
    }
    
    protected virtual void OnDisabledState()
    {
        _imgRaycasterTarget.raycastTarget = false;
        _imgAnimationTargets.ForEach(x => x.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f));
    }

    protected virtual void OnNormalState()
    {
        _imgRaycasterTarget.raycastTarget = true;
        _imgAnimationTargets.ForEach(x => x.transform.DOScale(Vector3.one * 0.95f, 0.2f));
    }

    protected virtual void OnHoverState()
    {
        StopAllCoroutines();
        _imgAnimationTargets.ForEach(x => x.transform.DOScale(Vector3.one, 0.2f));
    }

    private IEnumerator TweenScale(bool isUp)
    {
        var desiredScale = isUp ? 1f : 0.95f;
        var speed = 2f;
        var target = _imgAnimationTargets.FirstOrDefault();
        if (target == null) yield break;
        while (Math.Abs(target.transform.localScale.x - desiredScale) > 0.01f)
        {
            var amount = Time.deltaTime * (isUp ? 1f : -1f) * speed;
            var newScale = Mathf.Clamp(target.transform.localScale.x + amount, 0.95f, 1f);
            _imgAnimationTargets.ForEach(x => x.transform.localScale = (Vector3.one * newScale));
            yield return null;
        }
    }

    protected virtual void OnPressedState()
    {
    }

    protected virtual void OnToggledState()
    {
        _animator.SetBool(_isSelected, !_animator.GetBool(_isSelected));
    }

    protected virtual void OnSelectedState()
    {
    }

    protected virtual void OnDeselectedState()
    {
    }

    protected virtual void OnDimmedState()
    {
        _imgRaycasterTarget.raycastTarget = false;
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
        _animator.SetTrigger(_enter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetTrigger(_exit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _animator.SetTrigger(_up);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.SetTrigger(_down);
    }

    #endregion
}