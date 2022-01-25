using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIInfiniteScroll
{
    public class GetSiblingIndexStudy : MonoBehaviour
    {
        public string idx;

        private void Update()
        {
            idx = transform.GetSiblingIndex().ToString();
        }
    }

}
