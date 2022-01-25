using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIInfiniteScroll
{
    public class ScrollController : MonoBehaviour
    {
        public ScrollRect sr;
        private RectTransform rtContent;

        private void Awake()
        {
            rtContent = sr.content;
        }
        public void MoveToSpecificIndexOf(RectTransform target)
        {

        }

        private IEnumerator Scroll(RectTransform target, int destination)
        {
            while (target.GetSiblingIndex() != destination)
            {
                yield return null;
            }
        }
    }

}
