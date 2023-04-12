using System.Globalization;
using Unity.Netcode;
using UnityEngine;

namespace Game.Core
{
    public sealed class PlayerObject : NetworkBehaviour
    {
        private static PlayerObject _localPlayer;

        private readonly NetworkVariable<double> _balanceNetVar =
            new(0d, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        public static PlayerObject LocalPlayer => _localPlayer;

        public double Balance => _balanceNetVar.Value;


        public static event System.Action<PlayerObject> LocalPlayerSpawned;

        public event System.Action<double> BalanceChanged;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInit()
        {
            _localPlayer = null;
            LocalPlayerSpawned = null;
        }


        private void Awake()
        {
            //move to OnNetSpawn?
            _balanceNetVar.OnValueChanged += RethrowBalanceChangedEvent;
        }


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if(IsOwner)
            {
                _localPlayer = this;
                LocalPlayerSpawned?.Invoke(this);
            }
        }


        public void SetBalance(double value)
        {
            _balanceNetVar.Value = value;
        }

        public void RegisterEarning(double earning)
        {
            _balanceNetVar.Value += earning;
        }

        public void RegisterSpending(double spending)
        {
            _balanceNetVar.Value -= spending;
        }


        private void RethrowBalanceChangedEvent(double previousValue, double newValue)
        {
            BalanceChanged?.Invoke(newValue);
        }
    }

}
