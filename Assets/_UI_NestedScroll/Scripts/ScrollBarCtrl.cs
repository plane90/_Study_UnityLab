using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarCtrl : MonoBehaviour
{
    public RectTransform scrollbar;
    [MyBox.ButtonMethod]
    public void SetSizeWithCurrentAnchors()
    {
        scrollbar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10);
        scrollbar.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 220);
    }
}
