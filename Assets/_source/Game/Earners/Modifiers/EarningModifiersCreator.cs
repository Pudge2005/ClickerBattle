using UnityEngine;

namespace Game.Earners.Modifiers
{
#if UNITY_EDITOR
    [System.Serializable]
    internal sealed class EarningModifiersCreator
    {
        [SerializeField] private int _levelsCount;
        [SerializeField] private AnimationCurve _flatCurve;
        [SerializeField] private AnimationCurve _multCurve;
        [SerializeField] private AnimationCurve _buyPriceCurve;
        [SerializeField] private AnimationCurve _sellPriceCurve;


        public EarningModifier[] CreateMods()
        {
            EarningModifier[] mods = new EarningModifier[_levelsCount];

            for (int i = 0; i < _levelsCount; i++)
            {
                float t = (float)i / (float)(_levelsCount - 1);

                if (float.IsSubnormal(t))
                    t = 0f;

                mods[i] = new EarningModifier(_flatCurve.Evaluate(t),
                    _multCurve.Evaluate(t), _buyPriceCurve.Evaluate(t),
                    _sellPriceCurve.Evaluate(t));
            }

            return mods;
        }
    }
#endif
}
