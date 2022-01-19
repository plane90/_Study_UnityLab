using System.Diagnostics;
using UnityEngine;

public class CheckReferenceDemo_2 : MonoBehaviour
{
    [ReadOnlyOnInspector] public bool isChecked;
    public CheckReference checkReference;
    public CheckReferenceDemo_2 obj;

    public string str;
    public string str1;
    public string str2;
    public string str3;

    public string Str { get { return str; } }
    public enum TesterEnum { A, B, C, };
    public TesterEnum testerEnum;

    private void OnEnable()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //isChecked = checkReference.Check;

        //isChecked = checkReference.CheckFaster;

        isChecked = checkReference.CheckFaster2;
        sw.Stop();

        Stopwatch swNormal = new Stopwatch();
        swNormal.Start();
        if (obj.Str != "abc")
            swNormal.Stop();

        UnityEngine.Debug.Log("CR::" + sw.Elapsed.TotalMilliseconds + " Nor::" + swNormal.Elapsed.TotalMilliseconds);
    }
}