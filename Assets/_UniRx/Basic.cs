using System;
using UniRx;
using UnityEngine;

namespace _UniRx
{
    public class Basic : MonoBehaviour
    {
        // cf. 이벤트 트리거 = 옵저버블(피관찰자) = 발행자
        // cf. 이벤트 리스너 = 옵저버(관찰자) = 구독자
        private void Start()
        {
            // Subject가 옵저버의 등록 및 이벤트 발행을 수행한다.
            Subject<int> subject = new Subject<int>();
            
            // Subject가 IObservable을 통해 리스너를 등록한다.
            IObservable<int> trigger = subject;
            // Subscribe의 인자로 옵저버를 전달 받는다.
            trigger.Subscribe(x => Debug.Log($"a{x}"));
            trigger.Subscribe(x => Debug.Log($"b{x}"));
            
            // Subject가 IObserver를 통해 리스너를 참조 및 통지한다.
            IObserver<int> listeners = subject;
            // OnNext의 인자로 메시지를 전달하며 실행한다.
            listeners.OnNext(3);
        }
    }
}