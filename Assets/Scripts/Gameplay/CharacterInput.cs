using Photon.Pun;
using UnityEngine;
namespace Volley.Gameplay
{
    [RequireComponent(typeof(CharacterMotor))]
    public class CharacterInput : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        private CharacterMotor _characterMotor;
        private void Start()
        {
            _characterMotor = GetComponent<CharacterMotor>();
        }
        private void Update()
        {
            if (!_photonView.IsMine)
                return;

            float horizontalInput = -Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Movement(new Vector3(verticalInput, 0, horizontalInput), Input.GetButtonDown("Jump"));
        }
        public void Movement(Vector3 direction, bool isJumping)
        {
            _characterMotor.Move(direction, isJumping);
        }
    }
}