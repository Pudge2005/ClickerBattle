using Unity.Netcode;
using UnityEngine;

namespace Game.Core
{
    [DefaultExecutionOrder(-9010)]
    public sealed class GameManagerInitor : NetworkBehaviour
    {
        [SerializeField] private GameManager _gm;
        [SerializeField] private double _balanceToWin = 1000_000_000d;
        [SerializeField] private float _timeLimit = 4f * 60f;
        [SerializeField] private float _startCountDownTime = 3f;


        public override void OnNetworkSpawn()
        {
            if (IsServer)
                _gm.Init(_balanceToWin, _timeLimit, _startCountDownTime);
        }
    }

}
