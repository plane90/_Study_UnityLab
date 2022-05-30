// for uGUI(from 4.6)
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UniRx.Examples
{
    public class Sample13_ToDoApp : MonoBehaviour
    {
        // Open Sample13Scene. Set from canvas
        public Text Title;
        public InputField ToDoInput;
        public Button AddButton;
        public Button ClearButton;
        public GameObject TodoList;

        // prefab:)
        public GameObject SampleItemPrefab;

        private readonly ReactiveCollection<GameObject> _toDoElements = new ReactiveCollection<GameObject>();

        void Start()
        {
            // merge Button click and push enter key on input field.
            /* c --c-----c--c-->
             * e ----e-e---e--e>
             * k ----k-----k--->
             * m --m-m---m-mm-->
             * c: 마우스 클릭
             * e: ToDoInput.text 갱신
             * k: 키보드(Enter) 입력
             * m: merge
             */
            var submit = Observable.Merge(
                AddButton.OnClickAsObservable().Select(_ => ToDoInput.text),
                ToDoInput.OnEndEditAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Return)));

            // add to reactive collection
            submit.Where(x => x != "")
                  .Subscribe(x =>
                  {
                      ToDoInput.text = ""; // clear input field
                      var itemGO = Instantiate(SampleItemPrefab);
                      (itemGO.GetComponentInChildren(typeof(Text)) as Text).text = x;
                      _toDoElements.Add(itemGO);
                  });

            // Collection Change Handling
            _toDoElements.ObserveCountChanged().Subscribe(x => Title.text = "TODO App, ItemCount:" + x);
            _toDoElements.ObserveAdd().Subscribe(x =>
            {
                x.Value.transform.SetParent(TodoList.transform, false);
            });
            _toDoElements.ObserveRemove().Subscribe(x =>
            {
                Destroy(x.Value);
            });

            // Clear
            ClearButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var removeTargets = _toDoElements.Where(x => x.GetComponent<Toggle>().isOn).ToArray();
                    foreach (var item in removeTargets)
                    {
                        _toDoElements.Remove(item);
                    }
                });
        }
    }
}

#endif