using UnityEngine;

namespace Game.Earners
{
    [System.Serializable]
    public sealed class EarningModifier
    {
        private const bool _allowNegativeEarning = false;

        [SerializeField] private double _flat;
        [SerializeField] private double _mult;
        [SerializeField] private double _buyPrice;
        [SerializeField] private double _sellPrice;


        public EarningModifier(double flat, double mult,
            double buyPrice, double sellPrice)
        {
            _flat = flat;
            _mult = mult;
            _buyPrice = buyPrice;
            _sellPrice = sellPrice;
        }


        public double Flat => _flat;
        public double Mult => _mult;
        public double BuyPrice => _buyPrice;
        public double SellPrice => _sellPrice;


        public double Modify(double source)
        {
            double modified = (source + _flat) * (1d + _mult);

            if (modified < 0 && !_allowNegativeEarning)
                return 0;

            return modified;
        }
    }
}
