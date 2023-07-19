using UnityEngine;
namespace Volley.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMotor : MonoBehaviour
    {
        private const float GravityModifier = 2;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float rotationSpeed;
        private CharacterController _characterController;

        private float _gravity;
        private float _ySpeed;
        private Vector3 areaLimitMax;

        private Vector3 areaLimitMin;
        private Transform myTransform;
        private Quaternion targetedRotationAngle;

        private void Start()
        {
            _gravity = UnityEngine.Physics.gravity.y * GravityModifier;
            myTransform = transform;
            _characterController = GetComponent<CharacterController>();
        }

        public void SetLimitedArea(Vector3 minLimit, Vector3 maxLimit)
        {
            areaLimitMin = minLimit;
            areaLimitMax = maxLimit;
        }

        public void TeleportPlayer(Vector3 newPosition)
        {
            transform.position = newPosition;
        }
        public void Move(Vector3 direction, bool isJumping)
        {
            bool isGrounded = _characterController.isGrounded;

            if (isGrounded && _characterController.velocity.y < 0)
                _ySpeed = 0;

            if (isJumping && isGrounded)
            {
                _ySpeed = 0;
                _ySpeed += Mathf.Sqrt(jumpForce * -3.0f * _gravity);
            }

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (direction.magnitude > 0)
                targetedRotationAngle = Quaternion.Euler(0, angle, 0);

            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetedRotationAngle, rotationSpeed * Time.deltaTime);

            _ySpeed += _gravity * Time.deltaTime;
            _characterController.Move(new Vector3(direction.x * speed, _ySpeed, direction.z * speed) * Time.deltaTime);
            CheckAreaLimits();
        }

        private void CheckAreaLimits()
        {
            var position = myTransform.position;

            if (position.x > areaLimitMax.x)
            {
                position = new Vector3(areaLimitMax.x, position.y, position.z);
                myTransform.position = position;
            }
            if (position.x < areaLimitMin.x)
            {
                position = new Vector3(areaLimitMin.x, position.y, position.z);
                myTransform.position = position;
            }
            if (position.z > areaLimitMax.z)
            {
                position = new Vector3(position.x, position.y, areaLimitMax.z);
                myTransform.position = position;
            }
            if (position.z < areaLimitMin.z)
            {
                position = new Vector3(position.x, position.y, areaLimitMin.z);
                myTransform.position = position;
            }
        }
    }
}