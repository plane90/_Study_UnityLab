using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyOnInspectorAttribute))]
public class ReadOnlyOnInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        using (new EditorGUI.DisabledScope(true))
        {
            position = EditorGUI.PrefixLabel(position, label);
            EditorGUI.PropertyField(position, prop, GUIContent.none);
        }
    }
}