using UnityEngine;

public class CheckReferenceDemo_1 : MonoBehaviour
{
    [ReadOnlyOnInspector] public bool isChecked;
    public CheckReference checkReference;

    private void OnEnable()
    {
        isChecked = checkReference.Check;
    }
}
