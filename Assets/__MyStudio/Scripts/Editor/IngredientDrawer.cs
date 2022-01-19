using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Ingredient))]
public class IngredientDrawer : PropertyDrawer
{
    private SerializedProperty targetObj;
    private SerializedProperty name;
    private SerializedProperty amount;
    private SerializedProperty unit;
    private SerializedProperty targetObjCached;

    private float lineHeight = EditorGUIUtility.singleLineHeight;
    private float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    private const string targetObjName = "targetObj";
    private const string nameName = "name";
    private const string amountName = "amount";
    private const string unitName = "unit";
    private const string targetObjCachedName = "targetObjCached";

    private GUIStyle popupStyle;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        targetObj = property.FindPropertyRelative(targetObjName);
        name = property.FindPropertyRelative(nameName);
        amount = property.FindPropertyRelative(amountName);
        unit = property.FindPropertyRelative(unitName);
        targetObjCached = property.FindPropertyRelative(targetObjCachedName);

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }
        // Draw label
        position = EditorGUI.PrefixLabel(position, label);
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var targetObjRect = new Rect(position.x, position.y, position.width, lineHeight);
        var amountRect = new Rect(position.x, targetObjRect.yMax + verticalSpacing, 30, lineHeight);
        var unitRect = new Rect(position.x + 35, targetObjRect.yMax + verticalSpacing, 50, lineHeight);
        var nameRect = new Rect(position.x + 90, targetObjRect.yMax + verticalSpacing, position.width - 90, lineHeight);
        var popupButtonRect = nameRect;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(targetObjRect, targetObj, GUIContent.none);
        if (!targetObj.objectReferenceValue)
        {
            return;
        }
        if (!targetObj.objectReferenceValue != targetObjCached.objectReferenceValue)
        {
            name.stringValue = null;
            amount.intValue = 0;
            unit.enumValueIndex = 0;
        }
        if (GUI.Button(popupButtonRect, "", popupStyle))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("ADD"),
                targetObjCached.objectReferenceValue == targetObj.objectReferenceValue,
                UpdateSerializedProperty,
                new GenericeMenuUserData(property, Ingredient.IngredientUnit.Spoon, amount.intValue, name.stringValue));
            menu.ShowAsContext();
        }
        EditorGUI.PropertyField(amountRect, amount, GUIContent.none);
        EditorGUI.PropertyField(unitRect, unit, GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    private void UpdateSerializedProperty(object userData)
    {
        ((GenericeMenuUserData)userData).Assign();
    }

    struct GenericeMenuUserData
    {
        SerializedProperty property;
        Ingredient.IngredientUnit unit;
        int amount;
        string name;

        public GenericeMenuUserData(
            SerializedProperty property,
            Ingredient.IngredientUnit unit,
            int amount,
            string name)
        {
            this.property = property;
            this.unit = unit;
            this.amount = amount;
            this.name = name;
        }

        public void Assign()
        {
            var targetObj = property.FindPropertyRelative(targetObjName);
            var targetObjCached = property.FindPropertyRelative(targetObjCachedName);
            var name = property.FindPropertyRelative(nameName);
            var amount = property.FindPropertyRelative(amountName);
            var unit = property.FindPropertyRelative(unitName);
            targetObjCached.objectReferenceValue = targetObj.objectReferenceValue;
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var targetObj = property.FindPropertyRelative(targetObjName).objectReferenceValue;
        return base.GetPropertyHeight(property, label) +
            (targetObj != null ? lineHeight + verticalSpacing : 0);
    }
}
