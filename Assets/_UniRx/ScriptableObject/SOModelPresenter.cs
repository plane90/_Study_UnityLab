using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SOModelPresenter : MonoBehaviour
{
    [SerializeField] private SOModel _soModel;
    [SerializeField] private Text _view;
    private int i = 0;

    private void Start()
    {
        _soModel.Word.Subscribe(x => _view.text = x);
    }

    [MyBox.ButtonMethod()]
    public void Test()
    {
        i++;
        _soModel.Word.Value = i.ToString();
    }
}
