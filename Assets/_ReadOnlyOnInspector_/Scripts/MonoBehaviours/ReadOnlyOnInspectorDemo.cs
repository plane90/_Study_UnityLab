using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadOnlyOnInspectorDemo : MonoBehaviour
{
    public int publicIntegerNormal;
    private int privateIntegerNormal;
    [SerializeField] private int privateIntegerSerializedFieldNormal;

    [ReadOnlyOnInspector] public int publicInteger;
    [ReadOnlyOnInspector] private int privateInteger;
    [ReadOnlyOnInspector] [SerializeField] private int privateIntegerSerializedField;
}
