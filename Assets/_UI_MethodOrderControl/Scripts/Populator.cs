using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIMethodOrderControl
{
    public class Populator : MonoBehaviour
    {
        public GameObject target;

        private void OnEnable()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    var gameObject = Instantiate(target, transform);
            //    gameObject.GetComponent<Logger>().id = gameObject.transform.GetSiblingIndex();
            //}
        }
        
        //private IEnumerator OnEnable()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var gameObject = Instantiate(target, transform);
        //        gameObject.GetComponent<Logger>().id = gameObject.transform.GetSiblingIndex();
        //    }
        //}

        public IEnumerator Populate()
        {
            for (int i = 0; i < 10; i++)
            {
                var gameObject = Instantiate(target, transform);
                gameObject.GetComponent<Logger>().id = gameObject.transform.GetSiblingIndex();
                yield return null;
            }
        }

        //private void Start()
        //{
        //    Debug.LogError($"Populator On Start");
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var gameObject = Instantiate(target, transform);
        //        gameObject.GetComponent<Logger>().id = gameObject.transform.GetSiblingIndex();
        //    }
        //}
        private IEnumerator Start()
        {
            Debug.LogError($"Populator On Start");
            for (int i = 0; i < 10; i++)
            {
                var gameObject = Instantiate(target, transform);
                gameObject.GetComponent<Logger>().id = gameObject.transform.GetSiblingIndex();
                yield return null;
            }
        }
    }
}