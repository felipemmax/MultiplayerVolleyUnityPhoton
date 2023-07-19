using UnityEngine;
using UnityEngine.Events;
namespace Volley.Gameplay.Physics
{
    public class CollisionEvent : MonoBehaviour
    {
        [SerializeField] private string filterTag;
        [SerializeField] private UnityEvent<GameObject> onCollisionEnter;
        [SerializeField] private UnityEvent<GameObject> onTriggerEnter;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(filterTag))
                onCollisionEnter.Invoke(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(filterTag))
                onTriggerEnter.Invoke(other.gameObject);
        }
    }
}