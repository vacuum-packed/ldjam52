using UnityEngine;

namespace _ldjam.Scripts
{
    public class RobotController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask obstacleLayer;
        
        [SerializeField]
        private float velocity;

        private Rigidbody _rigidbody;
        private bool _isCollided;

        void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * .5f);
            Debug.DrawRay(transform.position, transform.up * .5f);
            
            if (_isCollided)
            {
                return;
            }

            _rigidbody.AddForce(transform.forward * velocity);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ReflectOnObstacle(collision);
        }

        private void ReflectOnObstacle(Collision collision)
        {
            if (collision.gameObject.layer != obstacleLayer)
            {
                return;
            }

            var contactPoint = collision.GetContact(0);
            Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.red, 10);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            var forward = transform.forward;
            var newDirection = Vector3.Reflect(forward, contactPoint.normal);
            Debug.DrawRay(contactPoint.point, newDirection, Color.green, 10);

            var lookRotation = Quaternion.LookRotation(newDirection);
            _rigidbody.rotation = lookRotation;
        }
    }
}