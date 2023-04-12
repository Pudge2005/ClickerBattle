using TMPro;
using UnityEngine;

namespace Game.Miscs
{
    public sealed class AmountSelectorUi : MonoBehaviour
    {
        public delegate void ValueChangedDelegate(int prevValue, int newValue);


        [System.Serializable]
        private sealed class DeltaChanger
        {
            [SerializeField] private int _delta;
            [SerializeField] private ButtonWithText _button;

            private AmountSelectorUi _parent;


            internal void Init(AmountSelectorUi parent)
            {
                _parent = parent;
                _button.ButtonComponent.onClick.AddListener(HandleClick);   
            }


            private void HandleClick()
            {
                _parent.RegisterChange(_delta);   
            }
        }


        [SerializeField] private int _min;
        [SerializeField] private int _max;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private DeltaChanger[] _changers;


        public int Value { get; set; }     
        public int Min { get => _min; set => _min = value; }
        public int Max { get => _max; set => _max = value; }


        public event ValueChangedDelegate ValueChanged;


        private void Awake()
        {
            foreach (var changer in _changers)
            {
                changer.Init(this);
            }
        }


        internal void RegisterChange(int delta)
        {
            int prevV = Value;
            Value += delta;
            _amountText.text = Value.ToString();
            ValueChanged?.Invoke(prevV, Value);
        }
    }
}
