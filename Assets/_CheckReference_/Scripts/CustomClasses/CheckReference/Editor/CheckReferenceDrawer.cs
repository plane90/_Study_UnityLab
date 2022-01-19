using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(CheckReference))]
public class CheckReferenceDrawer : PropertyDrawer
{
    private SerializedProperty targetObj;
    private SerializedProperty fieldOrPropName;
    private SerializedProperty checkRefType;
    private SerializedProperty declaringType;
    private SerializedProperty declaringTypeObj;
    private SerializedProperty targetObjCached;
    private SerializedProperty assemblyTypeName;

    private const string targetObjName = "targetObj";
    private const string fieldOrPropNameName = "fieldOrPropName";
    private const string checkRefTypeName = "checkReferenceType";
    private const string declaringTypeName = "declaringType";
    private const string declaringTypeObjName = "declaringTypeObj";
    private const string targetObjCachedName = "targetObjCached";
    private const string assemblyTypeNameName = "assemblyTypeName";

    private readonly float lineHeight = EditorGUIUtility.singleLineHeight;
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    private GUIStyle popupStyle;
    private Rect targetObjRect;
    private Rect popupButtonRect;
    private Rect fieldRect;
    private Rect fieldLabelRect;

    private int indent;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        targetObj = property.FindPropertyRelative(targetObjName);
        fieldOrPropName = property.FindPropertyRelative(fieldOrPropNameName);
        checkRefType = property.FindPropertyRelative(checkRefTypeName);
        declaringType = property.FindPropertyRelative(declaringTypeName);
        declaringTypeObj = property.FindPropertyRelative(declaringTypeObjName);
        targetObjCached = property.FindPropertyRelative(targetObjCachedName);
        assemblyTypeName = property.FindPropertyRelative(assemblyTypeNameName);

        label = EditorGUI.BeginProperty(position, label, property);
        SetLabelAndRect(position, label);
        //EditorGUI.BeginChangeCheck();
        indent = EditorGUI.indentLevel;
        if (EditorGUI.indentLevel > 0)
        {
            EditorGUI.indentLevel--;
        }
        EditorGUI.BeginChangeCheck();
        {
            EditorGUI.PropertyField(targetObjRect, targetObj, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                declaringType.stringValue = null;
                fieldOrPropName.stringValue = null;
                checkRefType.enumValueIndex = (int)CheckReference.CheckReferencType.None;
            }
        }

        if (!targetObj.objectReferenceValue || targetObj.hasMultipleDifferentValues)
        {
            return;
        }

        if (GUI.Button(popupButtonRect, "", popupStyle))
        {
            GenericMenu menu = new GenericMenu();
            if (targetObj.objectReferenceValue is GameObject)
            {
                menu.AddDisabledItem(new GUIContent("Component"));
                menu.AddSeparator("");

                var targetGO = targetObj.objectReferenceValue;
                AddFields(property, menu, typeof(GameObject), targetGO);
                AddProperties(property, menu, typeof(GameObject), targetGO);

                // comps = {"Test GameObject (UnityEngine.Transform)", "Test GameObject (TestBehaviour)" }
                var comps = (targetGO as GameObject).GetComponents<Component>();
                var typeCompGroups = comps.GroupBy(comp => comp.GetType());

                // cTypeCompGroup: Key = { TestBehaviour }, Element = { "Test GameObject (TestBehaviour)" }
                foreach (var typeCompGroup in typeCompGroups)
                {
                    AddFields(property, menu, typeCompGroup.Key, typeCompGroup.ElementAt(0));
                    AddProperties(property, menu, typeCompGroup.Key, typeCompGroup.ElementAt(0));
                }
            }
            else if (targetObj.objectReferenceValue is ScriptableObject)
            {
                menu.AddDisabledItem(new GUIContent("ScriptableObject"));
                menu.AddSeparator("");

                var targetSO = targetObj.objectReferenceValue;
                AddFields(property, menu, targetSO.GetType(), targetSO);
                AddProperties(property, menu, targetSO.GetType(), targetSO);
            }
            else if (targetObj.objectReferenceValue is MonoScript)
            {
                menu.AddDisabledItem(new GUIContent("MonoScript"));
                menu.AddSeparator("");

                var targetMS = targetObj.objectReferenceValue as MonoScript;
                AddFields(property, menu, targetMS.GetClass(), targetMS);
                AddProperties(property, menu, targetMS.GetClass(), targetMS);
            }
            else if (targetObj.objectReferenceValue is MonoBehaviour)
            {
                menu.AddDisabledItem(new GUIContent("MonoBehaviour"));
                menu.AddSeparator("");

                var targetMB = targetObj.objectReferenceValue;
                AddFields(property, menu, targetMB.GetType(), targetMB);
                AddProperties(property, menu, targetMB.GetType(), targetMB);
            }
            else
            {
                var isSelected = targetObjCached.objectReferenceValue == targetObj.objectReferenceValue;
                menu.AddItem(new GUIContent("Object"), isSelected, UpdateSerializedPropertyObject, property);
            }
            menu.DropDown(popupButtonRect);
        }
        OnGUI(property);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    property.serializedObject.ApplyModifiedProperties();
        //}
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    private void SetLabelAndRect(Rect position, GUIContent label)
    {
        fieldLabelRect = new Rect(position.xMin, position.y + lineHeight, position.xMax, lineHeight);
        position = EditorGUI.PrefixLabel(position, label);

        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        targetObjRect = new Rect(position.x, position.y, position.width, lineHeight);
        popupButtonRect =
            new Rect(position.x,
            targetObjRect.yMax + verticalSpacing,
            popupStyle.fixedWidth + popupStyle.margin.right,
            lineHeight);
        fieldRect = new Rect(popupButtonRect.xMax, popupButtonRect.y, position.width - popupButtonRect.width, lineHeight);
    }

    private void AddFields(SerializedProperty property, GenericMenu menu, Type declaringType, Object declaringTypeObj)
    {
        var path = declaringType.Name;
        menu.AddDisabledItem(new GUIContent(path + "/Field"));
        menu.AddSeparator(path + "/");

        var test = declaringType.GetMethods().ToList();
        var bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
        if (!(declaringTypeObj is MonoScript))
        {
            bindingFlags |= BindingFlags.Instance;
        }

        var fieldInfoes =
            declaringType.GetFields(bindingFlags).
            Where(fi => fi.FieldType.IsPrimitive | fi.FieldType.IsEnum | fi.FieldType.Equals(typeof(string))).ToList();

        foreach (var fieldInfo in fieldInfoes)
        {
            var isSelected =
                targetObj.objectReferenceValue == targetObjCached.objectReferenceValue &&
                this.declaringType.stringValue == fieldInfo.DeclaringType.AssemblyQualifiedName &&
                this.declaringTypeObj.objectReferenceValue == declaringTypeObj &&
                fieldOrPropName.stringValue == fieldInfo.Name;

            var fieldType = FindMatchingType(fieldInfo.FieldType);

            menu.AddItem(
                new GUIContent(path + "/" + GetFormattedTypeName(fieldInfo.FieldType) + " " + fieldInfo.Name),
                isSelected,
                UpdateSerializedProperty,
                new GenericeMenuUserData(property, fieldInfo, declaringTypeObj, fieldType));
        }
    }

    private void AddProperties(SerializedProperty property, GenericMenu menu, Type declaringType, Object declaringTypeObj)
    {
        var path = declaringType.Name;
        menu.AddDisabledItem(new GUIContent(path + "/Property"));
        menu.AddSeparator(path + "/");

        var bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
        if (!(declaringTypeObj is MonoScript))
        {
            bindingFlags |= BindingFlags.Instance;
        }

        var propertyInfoes =
            declaringType.GetProperties(bindingFlags).
            Where(pi => pi.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length == 0 &&
            pi.GetGetMethod() != null &&
            !pi.PropertyType.Equals(typeof(Vector2)) &&
            !pi.PropertyType.Equals(typeof(Vector3)) &&
            !pi.PropertyType.Equals(typeof(Quaternion)) &&
            !pi.PropertyType.Equals(typeof(UnityEngine.SceneManagement.Scene)) &&
            !pi.PropertyType.Equals(typeof(Rect)) &&
            !pi.PropertyType.Equals(typeof(Matrix4x4))).ToList();

        foreach (var propInfo in propertyInfoes)
        {
            var isSelected =
                targetObj.objectReferenceValue == targetObjCached.objectReferenceValue &&
                this.declaringType.stringValue == propInfo.DeclaringType.AssemblyQualifiedName &&
                this.declaringTypeObj.objectReferenceValue == declaringTypeObj &&
                fieldOrPropName.stringValue == propInfo.Name;

            var fieldType = FindMatchingType(propInfo.PropertyType);

            menu.AddItem(
                new GUIContent(path + "/" + GetFormattedTypeName(propInfo.PropertyType) + " " + propInfo.Name),
                isSelected,
                UpdateSerializedProperty,
                new GenericeMenuUserData(property, propInfo, declaringTypeObj, fieldType));
        }
    }

    private CheckReference.CheckReferencType FindMatchingType(Type fieldOrPropInfoType)
    {
        if (fieldOrPropInfoType == typeof(int))
        {
            return CheckReference.CheckReferencType.Int;
        }
        if (fieldOrPropInfoType == typeof(float))
        {
            return CheckReference.CheckReferencType.Float;
        }
        if (fieldOrPropInfoType == typeof(string))
        {
            return CheckReference.CheckReferencType.String;
        }
        if (fieldOrPropInfoType == typeof(bool))
        {
            return CheckReference.CheckReferencType.Bool;
        }
        if (fieldOrPropInfoType.IsEnum)
        {
            return CheckReference.CheckReferencType.Enum;
        }
        if (fieldOrPropInfoType == typeof(Object))
        {
            return CheckReference.CheckReferencType.Object;
        }
        if (fieldOrPropInfoType == typeof(Sprite))
        {
            return CheckReference.CheckReferencType.Sprite;
        }
        if (fieldOrPropInfoType == typeof(Texture))
        {
            return CheckReference.CheckReferencType.Texture;
        }
        if (fieldOrPropInfoType == typeof(Material))
        {
            return CheckReference.CheckReferencType.Material;
        }
        return CheckReference.CheckReferencType.Object;
    }

    private string GetFormattedTypeName(Type type)
    {
        var result = type.Name;
        if (type == typeof(string))
        {
            return "string";
        }
        if (type == typeof(bool))
        {
            return "bool";
        }
        if (type == typeof(int))
        {
            return "int";
        }
        if (type == typeof(float))
        {
            return "float";
        }
        if (result.StartsWith("System"))
        {
            return result.Substring("System".Length);
        }
        if (result.StartsWith("UnityEngine"))
        {
            return result.Substring("UnityEngine".Length);
        }
        return result;
    }

    private void UpdateSerializedProperty(object userData)
    {
        ((GenericeMenuUserData)userData).Assign();
    }

    struct GenericeMenuUserData
    {
        private SerializedProperty property;
        private FieldInfo fieldInfo;
        private PropertyInfo propertyInfo;
        private Object declaringTypeObj;
        private CheckReference.CheckReferencType checkRefType;

        public GenericeMenuUserData(
            SerializedProperty property,
            FieldInfo fieldInfo,
            Object declaringObj,
            CheckReference.CheckReferencType fieldType)
        {
            this.property = property;
            this.fieldInfo = fieldInfo;
            this.propertyInfo = null;
            this.declaringTypeObj = declaringObj;
            this.checkRefType = fieldType;
        }

        public GenericeMenuUserData(
            SerializedProperty property,
            PropertyInfo propertyInfo,
            Object declaringObj,
            CheckReference.CheckReferencType propertyType)
        {
            this.property = property;
            this.fieldInfo = null;
            this.propertyInfo = propertyInfo;
            this.declaringTypeObj = declaringObj;
            this.checkRefType = propertyType;
        }

        public void Assign()
        {
            var targetObj = property.FindPropertyRelative(targetObjName);
            var fieldOrPropName = property.FindPropertyRelative(fieldOrPropNameName);
            var checkRefType = property.FindPropertyRelative(checkRefTypeName);
            var declaringType = property.FindPropertyRelative(declaringTypeName);
            var declaringTypeObj = property.FindPropertyRelative(declaringTypeObjName);
            var targetObjCached = property.FindPropertyRelative(targetObjCachedName);
            var assemblyTypeName = property.FindPropertyRelative(assemblyTypeNameName);
            if (fieldInfo != null)
            {
                checkRefType.enumValueIndex = (int)this.checkRefType;
                fieldOrPropName.stringValue = fieldInfo.Name;
                declaringType.stringValue = fieldInfo.DeclaringType.AssemblyQualifiedName;
                declaringTypeObj.objectReferenceValue = this.declaringTypeObj;
                targetObjCached.objectReferenceValue = targetObj.objectReferenceValue;
                assemblyTypeName.stringValue = fieldInfo.FieldType.AssemblyQualifiedName;
            }
            else
            {
                checkRefType.enumValueIndex = (int)this.checkRefType;
                fieldOrPropName.stringValue = propertyInfo.Name;
                declaringType.stringValue = propertyInfo.DeclaringType.AssemblyQualifiedName;
                declaringTypeObj.objectReferenceValue = this.declaringTypeObj;
                targetObjCached.objectReferenceValue = targetObj.objectReferenceValue;
                assemblyTypeName.stringValue = propertyInfo.PropertyType.AssemblyQualifiedName;
            }
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    private void UpdateSerializedPropertyObject(object property)
    {
        var prop = property as SerializedProperty;
        prop.FindPropertyRelative(targetObjCachedName).objectReferenceValue =
            prop.FindPropertyRelative(targetObjName).objectReferenceValue;
        prop.FindPropertyRelative(checkRefTypeName).enumValueIndex = (int)CheckReference.CheckReferencType.Object;
        prop.serializedObject.ApplyModifiedProperties();
    }

    private void OnGUI(SerializedProperty property)
    {
        CheckReference.CheckReferencType ft = (CheckReference.CheckReferencType)checkRefType?.enumValueIndex;
        SerializedProperty fieldOrProperty = null;

        switch (ft)
        {
            case CheckReference.CheckReferencType.Int:
                fieldOrProperty = property.FindPropertyRelative("intProp");
                break;
            case CheckReference.CheckReferencType.Float:
                fieldOrProperty = property.FindPropertyRelative("floatProp");
                break;
            case CheckReference.CheckReferencType.String:
                fieldOrProperty = property.FindPropertyRelative("stringProp");
                break;
            case CheckReference.CheckReferencType.Bool:
                fieldOrProperty = property.FindPropertyRelative("boolProp");
                break;
            case CheckReference.CheckReferencType.Object:
                fieldOrProperty = property.FindPropertyRelative("objProp");
                break;
            case CheckReference.CheckReferencType.Sprite:
                fieldOrProperty = property.FindPropertyRelative("spriteProp");
                break;
            case CheckReference.CheckReferencType.Texture:
                fieldOrProperty = property.FindPropertyRelative("textureProp");
                break;
            case CheckReference.CheckReferencType.Material:
                fieldOrProperty = property.FindPropertyRelative("materialProp");
                break;
        }

        if (ft == CheckReference.CheckReferencType.Enum)
        {
            Type enumType = Type.GetType(assemblyTypeName.stringValue, false);
            if (enumType != null)
            {
                EditorGUI.indentLevel = indent;
                EditorGUI.LabelField(fieldLabelRect, new GUIContent(fieldOrPropName.stringValue ?? "Enum"));
                if (EditorGUI.indentLevel > 0)
                {
                    EditorGUI.indentLevel--;
                }
                string[] names = Enum.GetNames(enumType);
                var checkReference = property.FindPropertyRelative("enumProp");
                checkReference.intValue = EditorGUI.Popup(fieldRect, checkReference.intValue, names);
            }
        }
        else if (fieldOrProperty != null)
        {
            EditorGUI.indentLevel = indent;
            EditorGUI.LabelField(fieldLabelRect, new GUIContent(fieldOrPropName.stringValue ?? "Object"));
            if (EditorGUI.indentLevel > 0)
            {
                EditorGUI.indentLevel--;
            }
            EditorGUI.PropertyField(fieldRect, fieldOrProperty, GUIContent.none);
        }
        else
        {
            GUI.Box(fieldRect, GUIContent.none);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var targetObj = property.FindPropertyRelative(targetObjName).objectReferenceValue;
        return base.GetPropertyHeight(property, label) +
            (targetObj != null ? lineHeight + verticalSpacing : 0);
    }
}