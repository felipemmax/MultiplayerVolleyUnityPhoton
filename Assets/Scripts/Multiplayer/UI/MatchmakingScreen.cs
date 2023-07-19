using Photon.Pun;
using TMPro;
using UnityEngine;
namespace Volley.Multiplayer.UI
{
    public class MatchmakingScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] playerNames;
        [SerializeField] private GameObject startMatchButton;
        [SerializeField] private GameObject leaveRoomButton;
        [SerializeField] private GameObject findMatchButton;
        [SerializeField] private GameObject nickNameInput;
        [SerializeField] private TextMeshProUGUI statusText;
        public string localPlayerNickname;

        private void Start()
        {
            SetupFindMatchScreen();
        }
        public void FindMatchPressed()
        {
            if (localPlayerNickname.Length > 0)
                PhotonNetwork.NickName = localPlayerNickname;
            else
            {
                PhotonNetwork.NickName = "RandomPlayer " + Random.Range(1, 10000);
            }
        }

        public void SetupRoomScreen()
        {
            if (PhotonNetwork.IsMasterClient)
                startMatchButton.SetActive(true);

            findMatchButton.SetActive(false);
            nickNameInput.SetActive(false);
            leaveRoomButton.SetActive(true);
        }

        public void SetupFindMatchScreen()
        {
            nickNameInput.SetActive(true);
            startMatchButton.SetActive(false);
            findMatchButton.SetActive(true);
            leaveRoomButton.SetActive(false);
        }

        public void SetupName(string newName)
        {
            localPlayerNickname = newName;
        }

        public void AddLogText(string newText)
        {
            string oldText = statusText.text;
            statusText.text = newText + "\n" + oldText;
        }

        public void SetPlayerNames(string[] players)
        {
            for (int i = 0; i < playerNames.Length; i++)
            {
                if (i < players.Length)
                    playerNames[i].text = players[i];
                else
                {
                    playerNames[i].text = "Vazio";
                }
            }
        }
    }
}