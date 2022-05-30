// for uGUI(from 4.6)
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)

using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniRx.Examples
{
    public class Sample12_ReactiveProperty : MonoBehaviour
    {
        // Open Sample12Scene. Set from canvas
        public Button DamageButton;
        public Toggle Toggle;
        public InputField InputField;
        public Text HpText;
        public Slider Slider;

        // You can monitor/modify in inspector by SpecializedReactiveProperty
        public IntReactiveProperty IntRxProp = new IntReactiveProperty();

        private EnemyModel _enemyModel = new EnemyModel(1000);

        void Start()
        {
            // UnityEvent as Observable
            // (shortcut, MyButton.OnClickAsObservable())
            DamageButton.onClick.AsObservable().Subscribe(_ => _enemyModel.CurrentHp.Value -= 100);

            // Toggle, Input etc as Observable(OnValueChangedAsObservable is helper for provide isOn value on subscribe)
            // SubscribeToInteractable is UniRx.UI Extension Method, same as .interactable = x)
            Toggle.OnValueChangedAsObservable().SubscribeToInteractable(DamageButton);

            // input shows delay after 1 second
            /* e -e--e--e----e-->
             * d --d--d--d----d->
             * e: InputField.text 갱신
             * d: Delay 1초
             */
#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
            InputField.OnValueChangedAsObservable()
#else
            MyInput.OnValueChangeAsObservable()
#endif
                .Where(x => x != null)
                .Delay(TimeSpan.FromSeconds(1))
                .SubscribeToText(HpText); // SubscribeToText is UniRx.UI Extension Method

            // converting for human visibility
            Slider.OnValueChangedAsObservable()
                .SubscribeToText(HpText, x => Math.Round(x, 2).ToString());

            // from RxProp, CurrentHp changing(Button Click) is observable
            _enemyModel.CurrentHp.SubscribeToText(HpText);
            _enemyModel.IsDead.Where(isDead => isDead == true)
                .Subscribe(_ =>
                {
                    Toggle.interactable = DamageButton.interactable = false;
                });

            // initial text:)
            IntRxProp.SubscribeToText(HpText);
        }
    }

    // Reactive Notification Model
    public class EnemyModel
    {
        public IReactiveProperty<long> CurrentHp { get; private set; }

        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }

        public EnemyModel(int initialHp)
        {
            // Declarative Property
            CurrentHp = new ReactiveProperty<long>(initialHp);
            IsDead = CurrentHp.Select(x => x <= 0).ToReactiveProperty();
        }
    }
}

#endif