using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFocusing
{
    public class UIController : MonoBehaviour
    {
        public UIElementGroup togglesElementGroup;
        public UIElementGroup imagesElementGroup;

        public string keyToggle;
        [MyBox.ButtonMethod]
        public void TestTogglesElementGroup()
        {
            togglesElementGroup.uiElementMap[keyToggle].GetComponent<Toggle>().isOn = true;
        }
        public string keyImage;
        [MyBox.ButtonMethod]
        public void TestImagesElementGroup()
        {
            imagesElementGroup.uiElementMap[keyImage].GetComponent<Button>().Select();
        }
    }
}
