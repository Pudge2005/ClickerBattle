using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Miscs
{
    public sealed class ButtonWithText : MonoBehaviour
    {
        [SerializeField] private Button _buttonCmp;
        [SerializeField] private TMP_Text _textCmp;


        public Button ButtonComponent => _buttonCmp;
        public TMP_Text TextComponent => _textCmp;
    }
}
