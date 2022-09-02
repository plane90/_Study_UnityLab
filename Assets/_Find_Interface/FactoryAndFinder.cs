using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Find_Interface
{
    public class FactoryAndFinder : MonoBehaviour
    {
        [SerializeField] private int _count;
        [SerializeField] private Transform _root;
        [SerializeField] private Text _txtDebug;
        
        [MyBox.ButtonMethod()]
        public void CreateITargetImpl()
        {
            for (var i = 0; i < _count; i++)
            {
                var num = UnityEngine.Random.Range(0, 2);
                var go = new GameObject();
                if(num == 1) go.AddComponent<TargetImpl>();
            }
        }
        
        [MyBox.ButtonMethod()]
        public void CreateITargetImpl_Children()
        {
            for (var i = 0; i < _count; i++)
            {
                var num = UnityEngine.Random.Range(0, 2);
                var go = new GameObject();
                if(num == 1) go.AddComponent<TargetImpl>();
                go.transform.SetParent(_root);
            }
        }
        
        [MyBox.ButtonMethod()]
        public void FindViaLinq_OfType()
        {
            var sw = new Stopwatch();
            sw.Start();

            var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();
            
            sw.Stop();
            Debug.Log($"FindViaLinq_OfType: {sw.Elapsed.TotalMilliseconds}ms");
            _txtDebug.text = $"FindViaLinq_OfType: {sw.Elapsed.TotalMilliseconds}ms";
        }
        
        [MyBox.ButtonMethod()]
        public void FindViaLinq_SelectMany()
        {
            var sw = new Stopwatch();
            sw.Start();

            var targets = SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<ITarget>());
            
            sw.Stop();
            Debug.Log($"FindViaLinq_SelectMany: {sw.Elapsed.TotalMilliseconds}ms");
            _txtDebug.text = $"FindViaLinq_SelectMany: {sw.Elapsed.TotalMilliseconds}ms";
        }
        
        [MyBox.ButtonMethod()]
        public void FindViaGetComponentsInChildren()
        {
            var sw = new Stopwatch();
            var targets = new List<ITarget>(); 
            sw.Start();
            
            var goRoots = SceneManager.GetActiveScene().GetRootGameObjects();
            Array.ForEach(goRoots, x =>
            {
                targets.AddRange(x.GetComponentsInChildren<ITarget>());
            });
            
            sw.Stop();
            Debug.Log($"FindViaGetComponentsInChildren: {sw.Elapsed.TotalMilliseconds}ms");
            _txtDebug.text = $"FindViaGetComponentsInChildren: {sw.Elapsed.TotalMilliseconds}ms";
        }
        
        [MyBox.ButtonMethod()]
        public void FindViaGetComponentsInChildren_()
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var targets = FindInterfaces.Find<ITarget>();
            
            sw.Stop();
            Debug.Log($"FindViaGetComponentsInChildren: {sw.Elapsed.TotalMilliseconds}ms");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) FindViaLinq_OfType();
            if (Input.GetKeyDown(KeyCode.F2)) FindViaLinq_SelectMany();
            if (Input.GetKeyDown(KeyCode.F3)) FindViaGetComponentsInChildren();
        }
    }
}