using UnityEngine;

namespace Game.Core
{
    public sealed class BeginingCountDown : MonoBehaviour
    {
        private float _startDelay;
        private float _timeLeft;


        public float StartDelay => _startDelay;
        public float TimeLeft => _timeLeft;


        public event System.Action CountDownFinished;


        private void Awake()
        {
            //network should be inited already

            var cfg = GameManager.Config;
            _startDelay = cfg.StartTimeDelay;
            _timeLeft = _startDelay;
        }


        private void Update()
        {
            if ((_timeLeft -= Time.deltaTime) > 0)
                return;

            enabled = false;
            CountDownFinished?.Invoke();
        }
    }

}
