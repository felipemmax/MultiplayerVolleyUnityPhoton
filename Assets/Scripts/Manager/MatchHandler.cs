using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Volley.Gameplay;
using Volley.Gameplay.Physics;
using Volley.UI;
namespace Volley.Manager
{
    public class MatchHandler : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Ball ball;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject matchmakingScreen;

        [SerializeField] private int pointsToWin = 8;
        private GamePanel _gamePanel;
        private Playfield _playfield;

        private int leftScore;
        private int rightScore;

        private void Start()
        {
            ball = FindObjectOfType<Ball>();
            _gamePanel = FindObjectOfType<GamePanel>();
            _playfield = FindObjectOfType<Playfield>();
        }

        public void Score(Vector3 ballPosition)
        {
            if (ballPosition.z < _playfield.areaLimitationMaxLeftTeam.z && ballPosition.z > _playfield.areaLimitationMinLeftTeam.z && ballPosition.x < _playfield.areaLimitationMaxLeftTeam.x && ballPosition.x > _playfield.areaLimitationMinLeftTeam.x)
                rightScore++;
            else if (ballPosition.z < _playfield.areaLimitationMaxRightTeam.z && ballPosition.z > _playfield.areaLimitationMinRightTeam.z && ballPosition.x < _playfield.areaLimitationMaxRightTeam.x && ballPosition.x > _playfield.areaLimitationMinRightTeam.x)
                leftScore++;
            else if (ballPosition.z > 0)
                rightScore++;
            else
                leftScore++;

            Debug.Log(ballPosition + " " + leftScore + " " + rightScore);
            Debug.Log(_playfield.areaLimitationMaxLeftTeam.z + " " + _playfield.areaLimitationMinLeftTeam.z + " " + _playfield.areaLimitationMaxLeftTeam.x + " " + _playfield.areaLimitationMinLeftTeam.x);

            if (rightScore >= pointsToWin || leftScore >= pointsToWin)
                EndMatch();
            else
            {
                gameObject.GetPhotonView().RPC("UpdateScore", RpcTarget.All, leftScore.ToString(), rightScore.ToString());
                StartCoroutine(RestartRoutine());
            }
        }

        [PunRPC]
        public void UpdateScore(string leftScoreString, string rightScoreString)
        {
            _gamePanel.UpdateScore(leftScoreString, rightScoreString);
        }

        public void StartMatch()
        {
            if (PhotonNetwork.IsMasterClient)
                gameObject.GetPhotonView().RPC("StartMatchRPC", RpcTarget.All);
        }

        public void EndMatch()
        {
            gameObject.GetPhotonView().RPC("EndMatchRPC", RpcTarget.All, leftScore > rightScore ? "Esquerdo" : "Direito");
        }

        [PunRPC]
        public void EndMatchRPC(string winnerTeam)
        {
            _gamePanel.SetWinner(winnerTeam);
            StartCoroutine(CloseGame());
        }

        private IEnumerator CloseGame()
        {
            yield return new WaitForSeconds(5);
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        [PunRPC]
        public void StartMatchRPC()
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            SetNewPlayerTeam(player.GetComponent<VolleyPlayer>());
            RespawnBall();
            matchmakingScreen.SetActive(false);

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
        }

        private void SetNewPlayerTeam(VolleyPlayer volleyPlayer)
        {
            Team teamToSet = Team.Right;

            int localPlayerId = GetPlayerId(PhotonNetwork.LocalPlayer);

            if (localPlayerId % 2 == 0)
                teamToSet = Team.Left;

            volleyPlayer.SetupTeam(teamToSet, _playfield);
        }

        public static int GetPlayerId(Player player)
        {
            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].ActorNumber == player.ActorNumber)
                {
                    return i;
                }
            }

            return 0;
        }

        private IEnumerator RestartRoutine()
        {
            yield return new WaitForSeconds(4);
            DestroyBall();
            RespawnBall();
        }

        private void RespawnBall()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Vector3 randomSpawnPoint = _playfield.playerStartTeamStartPositions[Random.Range(0, 4)];
                Vector3 pointToSpawn = new(randomSpawnPoint.x, 7, randomSpawnPoint.z);
                ball = PhotonNetwork.Instantiate(ballPrefab.name, pointToSpawn, Quaternion.identity).GetComponent<Ball>();
            }
        }

        [PunRPC]
        private void DestroyBall()
        {
            PhotonNetwork.Destroy(ball.gameObject);
        }
    }
}