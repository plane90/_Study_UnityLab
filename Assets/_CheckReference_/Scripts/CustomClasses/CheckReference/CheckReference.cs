using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Diagnostics;


[System.Serializable]
public class CheckReference : ISerializationCallbackReceiver
{
    [Serializable]
    public enum CheckReferencType { Int, Float, String, Bool, Enum, Object, Sprite, Texture, Material, None, };
    [SerializeField] private Object targetObj;

    [SerializeField] private string fieldOrPropName;
    [SerializeField] private CheckReferencType checkReferenceType = CheckReferencType.None;
    [SerializeField] private Object declaringTypeObj;
    [SerializeField] private string declaringType;
    [SerializeField] private Object targetObjCached;

    [SerializeField] private int intProp;
    [SerializeField] private float floatProp;
    [SerializeField] private string stringProp;
    [SerializeField] private bool boolProp;
    [SerializeField] private int enumProp;
    [SerializeField] private Object objProp;
    [SerializeField] private Sprite spriteProp;
    [SerializeField] private Texture textureProp;
    [SerializeField] private Material materialProp;
    [SerializeField] private string assemblyTypeName;

    private CachedData cached = new CachedData();

    private int MyHashCode
    {
        get
        {
            switch (checkReferenceType)
            {
                case CheckReferencType.Int:
                    return intProp.GetHashCode();
                case CheckReferencType.Float:
                    return floatProp.GetHashCode();
                case CheckReferencType.String:
                    return stringProp.GetHashCode();
                case CheckReferencType.Bool:
                    return boolProp.GetHashCode();
                case CheckReferencType.Enum:
                    return enumProp.GetHashCode();
                case CheckReferencType.Object:
                    return objProp.GetHashCode();
                case CheckReferencType.Sprite:
                    return spriteProp.GetHashCode();
                case CheckReferencType.Texture:
                    return textureProp.GetHashCode();
                case CheckReferencType.Material:
                    return materialProp.GetHashCode();
                default:
                    return GetHashCode();
            }
        }
    }
    public bool Check
    {
        get
        {
            Stopwatch swType = new Stopwatch();
            Stopwatch swField = new Stopwatch();
            Stopwatch swProperty = new Stopwatch();
            Stopwatch swValue = new Stopwatch();
            Stopwatch swTotal = new Stopwatch();

            if (checkReferenceType == CheckReferencType.Object)
            {
                return objProp.GetHashCode() == targetObj.GetHashCode();
            }
            if (declaringType != null && !string.IsNullOrEmpty(fieldOrPropName))
            {
                swTotal.Start();
                swType.Start();
                var t1 = Type.GetType(declaringType, false);
                swType.Stop();
                swField.Start();
                var fi = t1?.GetField(fieldOrPropName) ?? null;
                swField.Stop();
                if (fi != null)
                {
                    swValue.Start();
                    var d = fi.GetValue(declaringTypeObj);
                    swValue.Stop();
                    swTotal.Stop();
                    //UnityEngine.Debug.Log("type" + swType.Elapsed.TotalMilliseconds + " f" + swField.Elapsed.TotalMilliseconds + " v" + swValue.Elapsed.TotalMilliseconds + " Total" + swTotal.Elapsed.TotalMilliseconds + " Check");
                    return MyHashCode == fi.GetValue(declaringTypeObj).GetHashCode();
                }
                swTotal.Restart();
                swType.Restart();
                var t2 = Type.GetType(declaringType, false);
                swType.Stop();
                swProperty.Start();
                var pi = t2?.GetProperty(fieldOrPropName) ?? null;
                swProperty.Stop();
                swValue.Start();
                if (pi != null)
                {
                    var d = pi.GetValue(declaringTypeObj);
                    swValue.Stop();
                    swTotal.Stop();
                    //UnityEngine.Debug.Log("type" + swType.Elapsed.TotalMilliseconds + " p" + swProperty.Elapsed.TotalMilliseconds + " v" + swValue.Elapsed.TotalMilliseconds + " Total" + swTotal.Elapsed.TotalMilliseconds + " Check");
                    return MyHashCode == pi.GetValue(declaringTypeObj).GetHashCode();
                }
            }
            return false;
        }
    }
    public bool CheckFaster
    {
        get
        {
            Stopwatch swValidCheck = new Stopwatch();
            Stopwatch swSetCache = new Stopwatch();
            Stopwatch swType = new Stopwatch();
            Stopwatch swField = new Stopwatch();
            Stopwatch swProperty = new Stopwatch();
            Stopwatch swBuild = new Stopwatch();
            Stopwatch swValue = new Stopwatch();
            Stopwatch swTotal = new Stopwatch();
            if (checkReferenceType == CheckReferencType.Object)
            {
                return objProp.GetHashCode() == targetObj.GetHashCode();
            }
            if (declaringType != null && !string.IsNullOrEmpty(fieldOrPropName))
            {
                swTotal.Start();
                swValidCheck.Start();
                if (cached.shouldReset(targetObj, fieldOrPropName, declaringTypeObj))
                {
                    swValidCheck.Stop();
                    swType.Start();
                    var t = Type.GetType(declaringType, false);
                    swType.Stop();
                    swField.Start();
                    var fi = t?.GetField(fieldOrPropName);
                    swField.Stop();
                    swProperty.Start();
                    var pi = t?.GetProperty(fieldOrPropName);
                    swProperty.Stop();
                    swSetCache.Start();
                    cached.Reset(t, pi, fi, targetObj, fieldOrPropName, declaringTypeObj);
                    swSetCache.Stop();
                }
                else
                {
                    swValidCheck.Stop();
                }
                if (cached.fi != null)
                {
                    swValue.Start();
                    var d = cached.fi.GetValue(declaringTypeObj);
                    swValue.Stop();
                    swTotal.Stop();
                    //UnityEngine.Debug.Log("type" + swType.Elapsed.TotalMilliseconds + " f" + swField.Elapsed.TotalMilliseconds + " v" + swValue.Elapsed.TotalMilliseconds + " Total" + swTotal.Elapsed.TotalMilliseconds + " ValidCheck" + swValidCheck.Elapsed.TotalMilliseconds + " Reset" + swSetCache.Elapsed.TotalMilliseconds + " CheckFaster");
                    return MyHashCode == cached.fi.GetValue(declaringTypeObj).GetHashCode();
                }
                if (cached.pi != null)
                {
                    swValue.Start();
                    var d = cached.pi.GetValue(declaringTypeObj);
                    swValue.Stop();
                    swTotal.Stop();
                    //UnityEngine.Debug.Log("type" + swType.Elapsed.TotalMilliseconds + " p" + swProperty.Elapsed.TotalMilliseconds + " v" + swValue.Elapsed.TotalMilliseconds + " Total" + swTotal.Elapsed.TotalMilliseconds + " ValidCheck" + swValidCheck.Elapsed.TotalMilliseconds + " Reset" + swSetCache.Elapsed.TotalMilliseconds + " CheckFaster");
                    return MyHashCode == cached.pi.GetValue(declaringTypeObj).GetHashCode();
                }
            }
            return false;
        }
    }

    public bool CheckFaster2
    {
        get
        {
            Stopwatch swValidCheck = new Stopwatch();
            Stopwatch swSetCache = new Stopwatch();
            Stopwatch swType = new Stopwatch();
            Stopwatch swField = new Stopwatch();
            Stopwatch swProperty = new Stopwatch();
            Stopwatch swBuild = new Stopwatch();
            Stopwatch swValue = new Stopwatch();
            Stopwatch swTotal = new Stopwatch();
            if (checkReferenceType == CheckReferencType.Object)
            {
                return objProp.GetHashCode() == targetObj.GetHashCode();
            }
            if (declaringType != null && !string.IsNullOrEmpty(fieldOrPropName))
            {
                swTotal.Start();
                swValidCheck.Start();
                if (cached.shouldReset(targetObj, fieldOrPropName, declaringTypeObj))
                {
                    swValidCheck.Stop();
                    swType.Start();
                    var t = Type.GetType(declaringType, false);
                    swType.Stop();
                    swField.Start();
                    var fi = t?.GetField(fieldOrPropName);
                    swField.Stop();
                    swProperty.Start();
                    var pi = t?.GetProperty(fieldOrPropName);
                    swProperty.Stop();
                    swSetCache.Start();
                    cached.Reset(t, pi, fi, targetObj, fieldOrPropName, declaringTypeObj);
                    swSetCache.Stop();
                }
                else
                {
                    swValidCheck.Stop();
                }
                if (cached.fi != null)
                {
                    swBuild.Start();
                    if (cached.GetFieldValue == null)
                    {
                        //cached.GetFieldValue = BuildGetFieldValue(cached.fi);
                    }
                    swBuild.Stop();
                    swValue.Start();
                    var d = cached.GetFieldValue(declaringTypeObj);
                    swValue.Stop();
                    swTotal.Stop();
                    //UnityEngine.Debug.Log("type" + swType.Elapsed.TotalMilliseconds + " f" + swField.Elapsed.TotalMilliseconds + " v" + swValue.Elapsed.TotalMilliseconds + " Total" + swTotal.Elapsed.TotalMilliseconds + " ValidCheck" + swValidCheck.Elapsed.TotalMilliseconds + " Reset" + swSetCache.Elapsed.TotalMilliseconds + " build" + swBuild.Elapsed.TotalMilliseconds + " CheckFaster2");
                    return MyHashCode == cached.GetFieldValue(declaringTypeObj).GetHashCode();
                }
                if (cached.pi != null)
                {
                    swBuild.Start();
                    if (cached.GetPropertyValue == null)
                    {
                        //cached.GetPropertyValue = BuildGetPropertyValue(cached.pi);
                    }
                    swBuild.Stop();
                    swValue.Start();
                    var d = cached.GetPropertyValue(declaringTypeObj);
                    swValue.Stop();
                    swTotal.Stop();
                    //UnityEngine.Debug.Log("type" + swType.Elapsed.TotalMilliseconds + " p" + swProperty.Elapsed.TotalMilliseconds + " v" + swValue.Elapsed.TotalMilliseconds + " Total" + swTotal.Elapsed.TotalMilliseconds + " ValidCheck" + swValidCheck.Elapsed.TotalMilliseconds + " Reset" + swSetCache.Elapsed.TotalMilliseconds + " build" + swBuild.Elapsed.TotalMilliseconds + " CheckFaster2");
                    return MyHashCode == cached.GetPropertyValue(declaringTypeObj).GetHashCode();
                }
            }
            return false;
        }
    }

    private struct CachedData
    {
        internal Type type;
        internal PropertyInfo pi;
        internal FieldInfo fi;
        internal Object targetObjCached;
        internal string fieldOrPropName;
        internal Object declaringTypeObj;
        internal Func<Object, object> GetPropertyValue;
        internal Func<Object, object> GetFieldValue;

        internal bool shouldReset(Object targetObj, string fieldOrPropName, Object declaringTypeObj)
        {
            return targetObjCached != targetObj ||
                this.fieldOrPropName != fieldOrPropName ||
                this.declaringTypeObj != declaringTypeObj;
        }

        internal void Reset(
            Type type,
            PropertyInfo pi,
            FieldInfo fi,
            Object targetObj,
            string fieldOrPropName,
            Object declaringTypeObj)
        {
            this.type = type;
            this.pi = pi;
            this.fi = fi;
            this.targetObjCached = targetObj;
            this.fieldOrPropName = fieldOrPropName;
            this.declaringTypeObj = declaringTypeObj;
            
            GetPropertyValue = (pi != null) ?
                CheckReferenceHelper.MakeFastPropertyGetter<object>(pi) :
                null;

        }
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        var t = Type.GetType(declaringType, false);
        var fi = t?.GetField(fieldOrPropName);
        var pi = t?.GetProperty(fieldOrPropName);
        cached.Reset(t, pi, fi, targetObj, fieldOrPropName, declaringTypeObj);
    }
}