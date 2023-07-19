using System.Collections;
using Photon.Pun;
using UnityEngine;
using Volley.Manager;
using Volley.UI;
namespace Volley.Gameplay.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        private MatchHandler _matchHandler;
        private PhotonView _photonView;
        private bool hasScored;
        private Rigidbody myRigidbody;

        private void OnEnable()
        {
            _photonView = gameObject.GetPhotonView();
            myRigidbody = GetComponent<Rigidbody>();
            _matchHandler = FindObjectOfType<MatchHandler>();
            StartCoroutine(ReleaseBallRoutine());
            FindObjectOfType<BallProjector>().SetupNewBall(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Floor") && hasScored == false)
            {
                if (PhotonNetwork.IsMasterClient)
                    _matchHandler.Score(transform.position);

                hasScored = true;
                myRigidbody.isKinematic = true;
            }
        }

        public void Hit(Vector3 force)
        {
            if (hasScored)
                return;

            _photonView.RPC("HitRPC", RpcTarget.All, force.x, force.y, force.z);
        }

        [PunRPC]
        public void HitRPC(float forceX, float forceY, float forceZ)
        {
            if (hasScored)
                return;

            myRigidbody.velocity = Vector3.zero;
            myRigidbody.AddForce(new Vector3(forceX, forceY, forceZ), ForceMode.Impulse);
        }

        private IEnumerator ReleaseBallRoutine()
        {
            yield return new WaitForSeconds(4);
            myRigidbody.isKinematic = false;
        }
    }
}