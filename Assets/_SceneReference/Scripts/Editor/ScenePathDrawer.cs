using SceneReference;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScenePath))]
public class ScenePathDrawer : PropertyDrawer
{
    private SceneAsset _curScene = null;
    private SerializedProperty _scenePath;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Init(property);
        var newScene = EditorGUI.ObjectField(position, property.name, _curScene, typeof(SceneAsset), false) as SceneAsset;
        var newScenePath = AssetDatabase.GetAssetPath(newScene);
        if (newScenePath.Equals(_scenePath.stringValue)) return;
        _scenePath.stringValue = newScenePath;
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    private void Init(SerializedProperty property)
    {
        _scenePath = property.FindPropertyRelative("_scenePath");
        _curScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(_scenePath.stringValue);
    }
}
