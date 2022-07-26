using UnityEditor;
using UnityEngine;
using UtilMyExtention;

[CustomPropertyDrawer(typeof(MySceneAttribute))]
public class MySceneDrawer : PropertyDrawer
{
    private SceneAsset _curScene;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "[Scene] 어트리뷰트는 string 타입 필드에서 사용됨.");
            return;
        }
        
        if (property.hasMultipleDifferentValues) EditorGUI.showMixedValue = true;

        _curScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(
            property.stringValue.IsNullOrEmpty() ?
            "Assets/_SceneProperty/SceneProperty.unity" :
            property.stringValue);
        
        var newScene = EditorGUILayout.ObjectField(property.name, _curScene, typeof(SceneAsset), false) as SceneAsset;
        var newScenePath = AssetDatabase.GetAssetPath(newScene);

        if (property.stringValue.Equals(newScenePath)) return;
        
        _curScene = newScene;
        property.stringValue = newScenePath;
        property.serializedObject.ApplyModifiedProperties();
    }
}
