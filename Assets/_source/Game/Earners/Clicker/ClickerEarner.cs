using Game.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Earners.Clicker
{
    public sealed class ClickerEarner : EarnerBase, IPointerDownHandler
    {
        [SerializeField] private AntiAutoClicker _antiAutoClicker;


        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsSpawned)
                return;

            if (!_antiAutoClicker.TryRegisterClick())
                return;

            var earning = GetEarning();
            ConfirmEarning(earning);
        }
    }

    public sealed class PassiveEarner : EarnerBase
    {
        [SerializeField, Min(0.01f)] private float _tickRate = 10f;

        private float _cdLeft;


        private void Update()
        {
            if (GameManager.GameState != GameState.InProgress)
                return;

            if ((_cdLeft -= Time.deltaTime) > 0)
                return;

            _cdLeft = 1f / _tickRate;

            var earningPerSec = GetEarning();
            var tickEarn = earningPerSec / _tickRate;
            ConfirmEarning(tickEarn);
        }
    }
}

