using System;
using Photon.Pun;
using UnityEngine;
using Volley.Gameplay.Physics;
using Volley.Manager;
using Random = UnityEngine.Random;
namespace Volley.Gameplay
{
    [RequireComponent(typeof(CharacterController), typeof(CharacterMotor))]
    public class VolleyPlayer : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Vector3 ballHitForce;
        [SerializeField] private Vector2 forceRandomRange;
        public Team team;
        [SerializeField] private CharacterMotor _motor;
        private Ball _ball;
        private CharacterController _characterController;
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }
        public void HitBall()
        {
            if (_ball == null)
                FindBall();

            if (!photonView.IsMine)
                return;

            Vector3 randomFactor = new(Random.Range(forceRandomRange.x, forceRandomRange.y), 1, Random.Range(forceRandomRange.x, forceRandomRange.y));
            Vector3 velocityNormalized = _characterController.velocity;
            _ball.Hit(new Vector3(1 * randomFactor.x * velocityNormalized.x * ballHitForce.x, ballHitForce.y, 1 * randomFactor.y + velocityNormalized.z * ballHitForce.z));
        }

        private void FindBall()
        {
            _ball = FindObjectOfType<Ball>();
        }

        public void SetupTeam(Team newTeam, Playfield playfield)
        {
            team = newTeam;

            switch (team)
            {
                case Team.Left:
                    _motor.SetLimitedArea(playfield.areaLimitationMinLeftTeam, playfield.areaLimitationMaxLeftTeam);
                    break;
                case Team.Right:
                    _motor.SetLimitedArea(playfield.areaLimitationMinRightTeam, playfield.areaLimitationMaxRightTeam);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _motor.TeleportPlayer(playfield.playerStartTeamStartPositions[MatchHandler.GetPlayerId(photonView.Owner)]);
        }
    }
}