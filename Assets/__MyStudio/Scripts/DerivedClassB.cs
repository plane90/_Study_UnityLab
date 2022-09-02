using UnityEngine;

public class DerivedClassB : BaseClass, ITester
{
    public void OnTester(string data)
    {
        Debug.Log(data);
    }
}
