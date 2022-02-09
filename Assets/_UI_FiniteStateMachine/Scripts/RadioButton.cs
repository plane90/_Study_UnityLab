using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFiniteStateMachine
{
    public class RadioButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool interactable = true;
        public RadioButtonGroup group;

        private bool curInteractable = true;

        [SerializeField]
        private bool isOn;
        public bool IsOn
        {
            get => isOn;
            set
            {
                if (!interactable)
                {
                    return;
                }
                isOn = value;
                if (IsOn)
                {
                    group.Notify(GetInstanceID());
                }
                //OnPropertyChanged("IsOn");
            }
        }

        [System.Serializable]
        public class Element { public Sprite sprite; public Color color; public bool enableColorChange; }
        [System.Serializable]
        public class ImageComponent
        {
            public Image component;
            public Element normal;
            public Element hover;
            public Element pressed;
            public Element selected;
            public Element disabled;
        }
        public ImageComponent image;

        [System.Serializable]
        public class TextComponent
        {
            public Text component;
            public Color normalColor = new Color(149f / 255f, 149f / 255f, 149f / 255f);
            public Color hoverColor = new Color(87f / 255f, 55f / 255f, 214f / 255f);
            public Color selectedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f);
            public Color disableColor = new Color(149f / 255f, 149f / 255f, 149f / 255f);
        }
        public TextComponent text;

        [System.Serializable] public class UnityEventBool : UnityEvent<bool> { };
        public UnityEventBool onValueChanged;
        public UnityEvent onInit;
        [System.Serializable] public class UnityEventColor : UnityEvent<Color> { };
        public UnityEventColor onSelected;
        public UnityEventColor onDeselected;

        [MyBox.ButtonMethod]
        public void SetOn()
        {
            IsOn = true;
        }

        public int GetId()
        {
            return GetInstanceID();
        }

        public void Init(bool isOn)
        {
            this.isOn = isOn;
            curIsOn = isOn;
            if (isOn)
            {
                UpdateSprite(image.selected);
                UpdateTextColor(text.selectedColor);
                onInit?.Invoke();
            }
            else
            {
                UpdateSprite(image.normal);
                UpdateTextColor(text.normalColor);
            }
        }

        public void InvokeValuechanged()
        {
            onValueChanged?.Invoke(IsOn);
        }

        private bool curIsOn = false;

        private void OnEnable()
        {
            group.Register(this);
            curIsOn = IsOn;

            if (!interactable)
            {
                UpdateSprite(image.disabled);
                UpdateTextColor(text.disableColor);
                curInteractable = false;
            }
            else
            {
                UpdateSprite(image.normal);
                UpdateTextColor(text.normalColor);
                curInteractable = true;
            }

            if (curIsOn)
            {
                UpdateSprite(image.selected);
                UpdateTextColor(text.selectedColor);
            }
            else
            {
                UpdateSprite(image.normal);
                UpdateTextColor(text.normalColor);
            }
        }

        private void OnDisable()
        {
            group.Unregister(this);
        }

        private void Update()
        {
            if (!curInteractable.Equals(interactable))
            {
                if (!interactable)
                {
                    UpdateSprite(image.disabled);
                    UpdateTextColor(text.disableColor);
                    curInteractable = false;
                }
                else
                {
                    UpdateSprite(image.normal);
                    UpdateTextColor(text.normalColor);
                    curInteractable = true;
                }
            }
        }

        private void UpdateSprite(Element e)
        {
            if (!curInteractable)
            {
                return;
            }
            if (image.component == null)
            {
                return;
            }
            if (e.sprite != null)
            {
                image.component.sprite = e.sprite;
            }
            if (e.enableColorChange)
            {
                image.component.color = e.color;
            }
        }

        private void UpdateTextColor(Color color)
        {
            if (!curInteractable)
            {
                return;
            }
            if (text.component != null)
            {
                text.component.color = color;
            }
        }

        public void OnClickedTo(int id)
        {
            if (!curInteractable)
            {
                return;
            }
            if (id.Equals(GetInstanceID()))
            {
                UpdateSprite(image.selected);
                UpdateTextColor(text.selectedColor);
                onSelected?.Invoke(text.selectedColor);
            }
            else
            {
                IsOn = false;
                UpdateSprite(image.normal);
                UpdateTextColor(text.normalColor);
                onDeselected?.Invoke(text.normalColor);
            }
            if (!curIsOn.Equals(IsOn))
            {
                curIsOn = IsOn;
                onValueChanged?.Invoke(IsOn);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (curInteractable)
            {
                IsOn = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateSprite(image.pressed);
            UpdateTextColor(text.hoverColor);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsOn)
            {
                UpdateSprite(image.hover);
                UpdateTextColor(text.hoverColor);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsOn)
            {
                if (image.selected == null)
                {
                    UpdateSprite(image.normal);
                    UpdateTextColor(text.normalColor);
                }
                else
                {
                    UpdateSprite(image.selected);
                    UpdateTextColor(text.selectedColor);
                }
            }
            else
            {
                UpdateSprite(image.normal);
                UpdateTextColor(text.normalColor);
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
