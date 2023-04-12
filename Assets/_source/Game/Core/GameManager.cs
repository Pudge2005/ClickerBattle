using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Game.Core
{
    [DefaultExecutionOrder(-9000)]
    public sealed class GameManager : NetworkBehaviour
    {
        public delegate void GameStateChangedDelegate(GameState previous, GameState current);


        private static GameManager _instance;

        private readonly NetworkVariable<GameState> _gameState = new();
        private readonly NetworkVariable<GameConfig> _config = new();

        private HashSet<ulong> _connectedClients;


        public static GameState GameState => _instance._gameState.Value;
        public static GameConfig Config => _instance._config.Value;


        public static event GameStateChangedDelegate GameStateChanged;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInit()
        {
            _instance = null;
        }


        private void Awake()
        {
            _instance = this;
            _gameState.OnValueChanged += (prev, cur) => GameStateChanged?.Invoke(prev, cur);
        }

        public override void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public void Init(double balanceToWin, float timeLimit, float startTimeDelay)
        {
            var cfg = new GameConfig
            {
                BalanceToWin = balanceToWin,
                TimeLimit = timeLimit,
                StartTimeDelay = startTimeDelay
            };

            _config.Value = cfg;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer)
                return;

            _connectedClients = new();
            NetworkManager.OnClientConnectedCallback += HandleClientConnectedCallback;
            NetworkManager.OnClientDisconnectCallback += HandleClientDisconnectedCallback;
        }

        private void HandleClientConnectedCallback(ulong clientId)
        {
            Debug.Log($"client id {clientId} connected");

            _connectedClients.Add(clientId);
            CheckForStartGame();
        }

        private void HandleClientDisconnectedCallback(ulong clientId)
        {
            Debug.Log($"client id {clientId} disconnected");

            _connectedClients.Remove(clientId);
        }

        private void CheckForStartGame()
        {
            if (_connectedClients.Count == 1)
                StartBeginGameCountDown();
        }

        private async void StartBeginGameCountDown()
        {
            //we are server
            _gameState.Value = GameState.BeginingCountDown;

            int waitTimeMs = (int)(_config.Value.StartTimeDelay * 1000);
            using var cts = new CancellationTokenSource();
            using var cts2 = UnityAsyncManager.CreateLinkedToOnDestroy(cts);

            await Task.Delay(waitTimeMs, cts2.Token);
            StartGame();
        }

        private async void StartGame()
        {
            _gameState.Value = GameState.InProgress;

            int waitTimeMs = (int)(_config.Value.StartTimeDelay * 1000);
            using var cts = new CancellationTokenSource();
            using var cts2 = UnityAsyncManager.CreateLinkedToOnDestroy(cts);

            try
            {
                await Task.Delay(waitTimeMs, cts2.Token);
            }
            catch (Exception ex)
            {
                Debug.LogError($"error stargame: {ex.Message}");
            }
            finally
            {
                Debug.Log("Start Game finally");
                //StopGame (or FinishGame)
            }
        }


    }

}
