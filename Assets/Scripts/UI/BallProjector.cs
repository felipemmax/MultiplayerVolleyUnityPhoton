using UnityEngine;
namespace Volley.UI
{
    public class BallProjector : MonoBehaviour
    {
        [SerializeField] private Transform ball;
        private Transform myTransform;

        private void Start()
        {
            myTransform = transform;
        }

        private void Update()
        {
            if (ball != null)
                myTransform.position = ball.position;
        }

        public void SetupNewBall(Transform newBall)
        {
            ball = newBall;
        }
    }
}