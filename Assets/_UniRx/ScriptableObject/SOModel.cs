using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Study_UniRx/SOModel")]
public class SOModel : ScriptableObject
{
    public SOModel()
    {
        Word = new ReactiveProperty<string>();
    }

    public ReactiveProperty<string> Word { get; private set; }
}
