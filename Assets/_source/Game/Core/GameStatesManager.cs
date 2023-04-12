using UnityEngine;

namespace Game.Core
{
    public sealed class GameStatesManager : MonoBehaviour
    {
        [SerializeField] private BeginingCountDown _beginingCountDownPrefab;


        private void Start()
        {
            GameManager.GameStateChanged += HandleGameStateChanged;
            HandleGameStateChanged(default, GameManager.GameState);
        }

        private void HandleGameStateChanged(GameState previous, GameState current)
        {
            switch (current)
            {
                case GameState.WaitingForPlayers:
                    break;
                case GameState.BeginingCountDown:
                    //countdown started
                    StartBeginingCountDown();
                    break;
                case GameState.InProgress:
                    //game started
                    break;
                case GameState.Finished:
                    break;
                default:
                    break;
            }
        }

        private void StartBeginingCountDown()
        {
            _ = Instantiate(_beginingCountDownPrefab);
        }
    }

}
