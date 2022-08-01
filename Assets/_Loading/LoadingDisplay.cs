using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _freezeChecker;
    [SerializeField] private Image _imgContext;

    public void SetContext(Sprite context)
    {
        _imgContext.sprite = context;
    }

    public void Show()
    {
        
    }

    public void Hide(float afterSec)
    {
        Destroy(gameObject, afterSec);
    }

    private void Update()
    {
        var desired = _freezeChecker.transform.rotation;
        desired *= Quaternion.AngleAxis(1f, Vector3.up);
        desired *= Quaternion.AngleAxis(1f, Vector3.forward);
        _freezeChecker.transform.rotation = desired;
    }
}
