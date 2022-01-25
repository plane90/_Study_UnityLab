using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIInfiniteScroll
{
    public class LocalToWorld : MonoBehaviour
    {
        public Vector3 localSpacePosition;
        public Vector3 worldSpacePosition;
        public Vector3 localSpacePositionViaInverse;
        public GameObject cube;

        private void Update()
        {
            localSpacePosition = transform.localPosition;
            // parent±âÁØ LocalSpace ÁÂÇ¥¸¦ WorldSpace ÁÂÇ¥·Î º¯È¯.
            worldSpacePosition = transform.parent.TransformPoint(localSpacePosition);
            if (cube != null)
            {
                cube.transform.position = worldSpacePosition;
            }
            // WorldSpace ÁÂÇ¥¸¦ parent ±âÁØ ·ÎÄÃ ÁÂÇ¥·Î º¯È¯
            localSpacePositionViaInverse = transform.parent.InverseTransformPoint(worldSpacePosition);
        }

        private Vector3 GetWidgetWorldPoint(RectTransform target)
        {
            //pivot position + item size has to be included
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y,
                0f);
            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }
        private Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
        {
            return target.InverseTransformPoint(worldPoint);
        }
    }
}