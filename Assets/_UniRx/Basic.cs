using System;
using UniRx;
using UnityEngine;

namespace _UniRx
{
    public class Basic : MonoBehaviour
    {
        private void Start()
        {
            Subject<int> subject = new Subject<int>();
            
            // Subject는 IObservable을 통해 이벤트 구독자를 모집한다. 
            IObservable<int> trigger = subject;
            trigger.Subscribe(x => Debug.Log($"a:{x}"));
            trigger.Subscribe(x => Debug.Log($"b:{x}"));
            
            // Subject는 IObserver를 통해 구독자를 참조 및 이벤트를 발행한다.
            IObserver<int> listeners = subject;
            listeners.OnNext(3);
        }
    }
}