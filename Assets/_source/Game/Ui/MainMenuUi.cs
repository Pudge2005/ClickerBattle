using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    public sealed class MainMenuUi : MonoBehaviour
    {
        [SerializeField] private Button _startAsHostButton;
        [SerializeField] private Button _startAsClientButton;


        private void Start()
        {
            _startAsHostButton.onClick.AddListener(StartAsHost);
            _startAsClientButton.onClick.AddListener(StartAsClient);
        }

        private void StartAsHost()
        {
            NetworkManager.Singleton.StartHost();
            Hide();
        }

        private void StartAsClient()
        {
            NetworkManager.Singleton.StartClient();
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}
