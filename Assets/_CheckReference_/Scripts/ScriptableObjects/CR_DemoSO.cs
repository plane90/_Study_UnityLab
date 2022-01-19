using UnityEngine;

[CreateAssetMenu(menuName ="CheckReferenceDemo/DataSetSO")]
public class CR_DemoSO : ScriptableObject
{
    public enum TestEnum { A, B, C,};
    public TestEnum testEnum;
    public string testSTr;
    public int testInt;
}