using System;
using System.Collections.Concurrent;
using System.Reflection;

public class CheckReferenceHelper
{
    // CallInnerDelegateMethod = 
    //"System.Func`2[System.Object,TResult] CallInnerDelegate[TClass,TResult](System.Func`2[TClass,TResult])"
    private static MethodInfo CallInnerDelegateMethod =
        typeof(CheckReferenceHelper).GetMethod(nameof(CallInnerDelegate), BindingFlags.NonPublic | BindingFlags.Static);

    private static ConcurrentDictionary<string, Delegate> cache = new ConcurrentDictionary<string, Delegate>();

    public static Func<object, TResult> MakeFastPropertyGetter<TResult>(PropertyInfo pi)
        => cache.GetOrAdd(pi.DeclaringType + pi.Name, key =>
        {
            // getMethod = "System.String get_Str()"
            var getMethod = pi.GetMethod;
            // declaringType = {CheckReferenceDemo_2}
            var declaringType = pi.DeclaringType;
            // tResultType = {System.Object}
            var tResultType = typeof(TResult);
            // genericGetMethodType = {System.Func`2[CheckReferenceDemo_2,System.Object]}
            var genericGetMethodType = typeof(Func<,>).MakeGenericType(declaringType, tResultType);
            // getMethodDelegate = {System.Func<CheckReferenceDemo_2, object>}
            var getMethodDelegate = getMethod.CreateDelegate(genericGetMethodType);
            // callInnerGenricMethod =
            // "System.Func`2[System.Object,System.Object] CallInnerDelegate[CheckReferenceDemo_2,Object](System.Func`2[CheckReferenceDemo_2,System.Object])"
            var callInnerGenericMethod = CallInnerDelegateMethod.MakeGenericMethod(declaringType, tResultType);
            // result = {System.Func<object, object>}
            var result = callInnerGenericMethod.Invoke(null, new[] { getMethodDelegate });

            return result as Func<object, TResult>;
        }) as Func<object, TResult>;

    // WrapperFunction
    private static Func<object, TResult> CallInnerDelegate<TClass, TResult>(Func<TClass, TResult> deleg)
        => instance => deleg((TClass)instance);
}

