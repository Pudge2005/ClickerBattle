using UnityEngine;
using Game.Core;
using System;
using Game.Miscs;
using TMPro;

namespace Game.Earners
{
    public sealed class PlayerEarnerUi : MonoBehaviour
    {
        [SerializeField] private EarnerBase _earner;

        [SerializeField] private AmountSelectorUi _lvlsAmountSelector;
        [SerializeField] private ButtonWithText _buyGoodLvlsBtn;
        [SerializeField] private TMP_Text _buyGoodLvlsCostText;

        [SerializeField] private TMP_Text _goodLvlText;
        [SerializeField] private TMP_Text _badLvlText;

        private bool _canBuyLvls;
        private int _cachedLvlsAmount;
        private double _cachedBuyLvlsCost;


        private void Awake()
        {
            _earner.GoodLvlChanged += HandleGoodLvlChanged;
            _earner.BadLvlChanged += HandleBadLvlChanged;

            _lvlsAmountSelector.ValueChanged += HandleValueChanged;
            _buyGoodLvlsBtn.ButtonComponent.onClick.AddListener(BuyGoodLvls);

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
        }

        private void HandleGoodLvlChanged(int prevValue, int newValue)
        {
            _goodLvlText.text = newValue.ToString();
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
            _cachedLvlsAmount = newValue;
            _cachedBuyLvlsCost = GetGoodLvlsBuyCost(_cachedLvlsAmount);
            UpdateBuyButtonState();
        }

        private void UpdateBuyButtonState()
        {
            var player = PlayerObject.LocalPlayer;
            SetBuyButtonState(player != null && player.Balance >= _cachedBuyLvlsCost);
        }

        private void SetBuyButtonState(bool state)
        {
            _canBuyLvls = state;
            _buyGoodLvlsBtn.ButtonComponent.interactable = state;
        }


        private void BuyGoodLvls()
        {
            if (!_canBuyLvls)
            {
                Debug.LogError("attempt to buy lvls when unable to");
                return;
            }

            //block until server update
            SetBuyButtonState(false);
            PlayerObject.LocalPlayer.RegisterSpending(_cachedBuyLvlsCost);
            _earner.SetGoodLevelServerRpc(_cachedLvlsAmount);
        }

        private double GetGoodLvlsBuyCost(int lvls)
        {
            if (lvls < 0)
                throw new ArgumentException($"negative arg: {lvls}", nameof(lvls));

            lvls = Mathf.Clamp(lvls, 0, _earner.MaxGoodLvl - _earner.GoodLvl);

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
