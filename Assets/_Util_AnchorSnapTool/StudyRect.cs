using UnityEngine;

namespace UtilAnchorSnapTool
{
    public class StudyRect : MonoBehaviour
    {
        private RectTransform _rtTarget;

        private void OnValidate()
        {
            _rtTarget ??= GetComponent<RectTransform>();
            elapsed = 0f;
        }

        private float elapsed = 0f;
        private void Update()
        {
            elapsed += Time.deltaTime;
            //_rtTarget.offsetMin = new Vector2(left, _rtTarget.offsetMax.x);
            //Debug.Log($"offsetMin: {_rtTarget.anchoredPosition.x}, offsetMax: {_rtTarget.anchoredPosition.y}, ");
            Debug.Log($"offsetMin: {_rtTarget.offsetMin}, offsetMax: {_rtTarget.offsetMax}, ");
        }

        [MyBox.ButtonMethod()]
        public void SetAnchorPos()
        {
            _rtTarget.anchorMin = Vector2.zero;
            _rtTarget.anchorMax = Vector2.one;
        }

        private Rect parentRect;
        [MyBox.ButtonMethod()]
        public void SetParentRect()
        {
            parentRect = _rtTarget.parent.GetComponent<RectTransform>().rect;
            Debug.Log($"parentRect: {parentRect}");
        }
    
        [MyBox.ButtonMethod()]
        public void SetAnchorSnap()
        {
            parentRect = _rtTarget.parent.GetComponent<RectTransform>().rect;
            var newAnchorMin = new Vector2(_rtTarget.offsetMin.x / parentRect.width, _rtTarget.offsetMin.y / parentRect.height);
            var newAnchorMax = new Vector2((_rtTarget.offsetMax.x + parentRect.width) / parentRect.width, (_rtTarget.offsetMax.y + parentRect.height) / parentRect.height);
            _rtTarget.anchorMin = newAnchorMin;
            _rtTarget.anchorMax = newAnchorMax;
        }
    
        [MyBox.ButtonMethod()]
        public void SetAnchorOffset()
        {
            parentRect = _rtTarget.parent.GetComponent<RectTransform>().rect;
            _rtTarget.offsetMin = new Vector2(0f, 0f);
            _rtTarget.offsetMax = new Vector2(0f, 0f);
        }
    }
}