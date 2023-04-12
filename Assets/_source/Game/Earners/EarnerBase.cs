using UnityEngine;
using Unity.Netcode;
using Game.Core;

namespace Game.Earners
{
    public abstract class EarnerBase : NetworkBehaviour
    {
        public delegate void EarningRegisteredDelegate(EarnerBase earner, double earning);

        public delegate void LvlChangedDelegate(int prevValue, int newValue);


        [SerializeField] private EarnerSo _so;

        private readonly NetworkVariable<int> _goodLvl = new();
        private readonly NetworkVariable<int> _badLvl = new();

        private int _maxGoodLvl;
        private int _maxBadLvl;

        public EarnerSo SO => _so;

        public int MaxGoodLvl => _maxGoodLvl;
        public int MaxBadLvl => _maxBadLvl;

        public int GoodLvl => _goodLvl.Value;
        public int BadLvl => _badLvl.Value;


        public event EarningRegisteredDelegate EarningRegistered;

        public event LvlChangedDelegate GoodLvlChanged;
        public event LvlChangedDelegate BadLvlChanged;


        protected virtual void Start()
        {
            Init(_so);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsClient)
            {
                _goodLvl.OnValueChanged += (prevV, newV) => GoodLvlChanged?.Invoke(prevV, newV);
                _badLvl.OnValueChanged += (prevV, newV) => BadLvlChanged?.Invoke(prevV, newV);
            }
        }

        public void Init(EarnerSo so)
        {
            _so = so;
            _maxGoodLvl = so.GoodLevels.Count;
            _maxBadLvl = so.BadLevels.Count;
        }


        [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
        public void SetGoodLevelServerRpc(int lvl)
        {
            _goodLvl.Value = lvl;
        }


        [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
        public void SetBadLevelServerRpc(int lvl)
        {
            _badLvl.Value = lvl;
        }


        public double GetEarning()
        {
            var goodLvl = GoodLvl;

            if (goodLvl < 0)
                return 0f;

            var positive = _so.GoodLevels[goodLvl];
            double flat = positive.Flat;
            double mult = positive.Mult;

            var badLvl = BadLvl;

            if (badLvl >= 0)
            {
                var negative = _so.BadLevels[badLvl];
                flat += negative.Flat;
                mult += negative.Mult;
            }

            double earning = flat * (1d + mult);

            if (earning < 0)
                earning = 0;

            return earning;
        }


        public void ConfirmEarning(double earning)
        {
            if (double.IsSubnormal(earning))
                return;

            PlayerObject.LocalPlayer.RegisterEarning(earning);
            EarningRegistered?.Invoke(this, earning);
        }

    }
}
