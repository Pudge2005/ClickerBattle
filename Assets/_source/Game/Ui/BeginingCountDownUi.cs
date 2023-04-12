using Game.Core;
using TMPro;
using UnityEngine;

namespace Game.Ui
{
    [RequireComponent(typeof(BeginingCountDown))]
    public sealed class BeginingCountDownUi : MonoBehaviour
    {
        [SerializeField] private float _fadeTime = 0.7f;
        [SerializeField] private TMP_Text _countDownText;

        private BeginingCountDown _cd;
        private bool _fading;
        private float _fadeTimeLeft;


        private void Awake()
        {
            _cd = GetComponent<BeginingCountDown>();
            _cd.CountDownFinished += HandleCountDownFinished;
        }

        private void Update()
        {
            if (_fading)
            {
                _fadeTime -= Time.deltaTime;

                //t is inversed (so it is not t, it is alpha)
                //todo: make t = 1f - t if more complex
                //evaluations needed
                float t = _fadeTime / _fadeTimeLeft;

                if (float.IsSubnormal(t))
                {
                    Destroy(gameObject);
                    return;
                }

                Color color = _countDownText.color;
                color.a = t;
                _countDownText.color = color;
            }
            else
            {
                _countDownText.text = _cd.TimeLeft.ToString("N2");
            }
        }


        private void HandleCountDownFinished()
        {
            _fading = true;
            _fadeTimeLeft = _fadeTime;
        }
    }

}
