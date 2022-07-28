using UnityEngine;

namespace UtilAnchorSnapTool
{
    [ExecuteInEditMode]
    public class MyAnchorSnapTool : MonoBehaviour
    {
        private Rect _parent;
        private RectTransform _target;
    
        // target의 Anchor가 Min(0,0), Max(1,1)으로 미리 설정이 되어있어야 정상 동작함.
        private void Start()
        {
#if UNITY_EDITOR
            _target = GetComponent<RectTransform>();
            _parent = _target.parent.GetComponent<RectTransform>().rect;
            var newAnchorMin = new Vector2(_target.offsetMin.x / _parent.width, _target.offsetMin.y / _parent.height);
            var newAnchorMax = new Vector2((_target.offsetMax.x + _parent.width) / _parent.width, (_target.offsetMax.y + _parent.height) / _parent.height);
            _target.anchorMin = newAnchorMin;
            _target.anchorMax = newAnchorMax;
            _target.offsetMin = new Vector2(0f, 0f);
            _target.offsetMax = new Vector2(0f, 0f);
#endif
        }
    }
}