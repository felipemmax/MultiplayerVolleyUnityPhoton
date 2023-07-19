using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Volley.Manager;
using Volley.Multiplayer.UI;
namespace Volley.Multiplayer
{
    public class MatchMultiplayer : MonoBehaviourPunCallbacks
    {

        public int playerTTL = -1;

        public byte MaxPlayers = 4;
        private MatchHandler _matchHandler;
        private MatchmakingScreen _matchmakingScreen;

        private void Start()
        {
            ConnectNow();

            _matchHandler = FindObjectOfType<MatchHandler>();
            _matchmakingScreen = FindObjectOfType<MatchmakingScreen>();
        }

        public void ConnectNow()
        {
            Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");


            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = 1 + "." + SceneManagerHelper.ActiveSceneBuildIndex;

        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

            RoomOptions roomOptions = new() { MaxPlayers = MaxPlayers };
            if (playerTTL >= 0)
                roomOptions.PlayerTtl = playerTTL;

            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            _matchmakingScreen.AddLogText("Entrou na sala " + PhotonNetwork.CurrentRoom.Name);
            _matchmakingScreen.SetupRoomScreen();
            UpdatePlayerNames();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _matchmakingScreen.AddLogText("Jogador entrou " + newPlayer.NickName);
            UpdatePlayerNames();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _matchmakingScreen.AddLogText("Jogador Saiu " + otherPlayer.NickName);
            UpdatePlayerNames();
        }

        public override void OnLeftRoom()
        {
            _matchmakingScreen.AddLogText("Você saiu da sala");
            _matchmakingScreen.SetupFindMatchScreen();
            UpdatePlayerNames();
        }

        private void UpdatePlayerNames()
        {
            Player[] players = PhotonNetwork.PlayerList;
            List<string> playerNames = new();
            playerNames.AddRange(players.Select(t => t.NickName));
            _matchmakingScreen.SetPlayerNames(playerNames.ToArray());
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}