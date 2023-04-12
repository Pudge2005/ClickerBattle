using UnityEngine;

namespace Game.Earners.Clicker
{
    public sealed class AntiAutoClicker : MonoBehaviour
    {
        [SerializeField] private float _maxAllowedCps = 13f;

        private float _blockTimeLeft;
        private bool _canRegisterClick;


        private void Update()
        {
            _blockTimeLeft -= Time.deltaTime;

            if (_blockTimeLeft <= 0)
            {
                _canRegisterClick = true;
                enabled = false;
            }
        }


        //todo: move to chain-checkers
        public bool TryRegisterClick()
        {
            if (_canRegisterClick)
            {
                _blockTimeLeft = 1f / _maxAllowedCps;
                enabled = true;
                return true;
            }

            return false;
        }
    }
}
