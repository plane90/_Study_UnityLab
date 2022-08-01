using System;
using UnityEngine;

namespace SceneReference
{
    [Serializable]
    public class ScenePath
    {
        [SerializeField] private string _scenePath = "empty";

        public override string ToString() => _scenePath;

        // 사용자 정의 암시적 형변환 연산자
        public static implicit operator string(ScenePath x) => x._scenePath;
    }
}