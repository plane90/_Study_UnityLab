using System;
using UnityEngine;
// Custom serializable class
[Serializable]
public class Ingredient : ISerializationCallbackReceiver
{
    public string name;
    [SerializeField] private int amount = 1;
    public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
    public IngredientUnit unit;
    [SerializeField] private UnityEngine.Object targetObj;
    [SerializeField] private UnityEngine.Object targetObjCached;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}
