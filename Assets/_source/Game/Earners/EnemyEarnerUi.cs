using Game.Core;
using Game.Miscs;
using System;
using TMPro;
using UnityEngine;

namespace Game.Earners
{
    public sealed class EnemyEarnerUi : MonoBehaviour
    {
        [SerializeField] private EarnerBase _earner;

        [SerializeField] private AmountSelectorUi _lvlsAmountSelector;
        [SerializeField] private ButtonWithText _buyBadLvlsBtn;
        [SerializeField] private TMP_Text _buyBadLvlsCostText;
        [SerializeField] private TMP_Text _badLvlText;

        private bool _canBuyLvls;
        private int _cachedAmount;
        private double _cachedCost;


        private void Awake()
        {
            _earner.BadLvlChanged += HandleBadLvlChanged;
            _lvlsAmountSelector.ValueChanged += HandleValueChanged;
            _buyBadLvlsBtn.ButtonComponent.onClick.AddListener(BuyBadLvls);

            if (PlayerObject.LocalPlayer == null)
            {
                PlayerObject.LocalPlayerSpawned += HandleLocalPlayerSpawned;
            }
            else
            {
                HandleLocalPlayerSpawned(PlayerObject.LocalPlayer);
            }
        }

        private void HandleBadLvlChanged(int prevValue, int newValue)
        {
            _badLvlText.text = newValue.ToString();
            UpdateBuyButtonState();
        }

        private void HandleLocalPlayerSpawned(PlayerObject localPlayer)
        {
            localPlayer.BalanceChanged += HandlePlayerBalanceChanged;
            HandlePlayerBalanceChanged(localPlayer.Balance);
        }

        private void HandlePlayerBalanceChanged(double balance)
        {
            UpdateBuyButtonState();
        }

        private void HandleValueChanged(int prevValue, int newValue)
        {
            _cachedAmount = newValue;
            _cachedCost = GetBadLvlsBuyCost(_cachedAmount);
            UpdateBuyButtonState();
        }

        private void UpdateBuyButtonState()
        {
            var player = PlayerObject.LocalPlayer;
            SetBuyButtonState(player != null && player.Balance >= _cachedCost);
        }

        private void SetBuyButtonState(bool state)
        {
            _canBuyLvls = state;
            _buyBadLvlsBtn.ButtonComponent.interactable = state;
        }


        private void BuyBadLvls()
        {
            if (!_canBuyLvls)
            {
                Debug.LogError("attempt to buy lvls when unable to");
                return;
            }

            //block until server update
            SetBuyButtonState(false);
            PlayerObject.LocalPlayer.RegisterSpending(_cachedCost);
            _earner.SetBadLevelServerRpc(_cachedAmount);
        }

        private double GetBadLvlsBuyCost(int lvls)
        {
            if (lvls < 0)
                throw new ArgumentException($"negative arg: {lvls}", nameof(lvls));

            lvls = Mathf.Clamp(lvls, 0, _earner.MaxBadLvl - _earner.BadLvl);

            if (lvls == 0)
                return 0d;

            double cost = 0d;
            var mods = _earner.SO.GoodLevels;

            for (int i = 0, lvl = _earner.GoodLvl + 1; i < lvls; ++i, ++lvls)
            {
                cost += mods[lvl].BuyPrice;
            }

            return cost;
        }
    }
}
