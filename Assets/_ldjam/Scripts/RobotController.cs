using UnityEngine;

namespace _ldjam.Scripts
{
    public class RobotController : MonoBehaviour
    {
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
            if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
            {
                return;
            }

            var contactPoint = collision.GetContact(0);
            Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.red, 10);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            var forward = transform.forward;
            var forward2D = new Vector2(forward.x, forward.z);
            var normal2D = new Vector2(contactPoint.normal.x, contactPoint.normal.z);
            var newDirection2D = Vector2.Reflect(forward2D, normal2D);
            var newDirection = new Vector3(newDirection2D.x, 0, newDirection2D.y);
            // var newDirection = Vector3.Reflect(forward, contactPoint.normal);
            Debug.DrawRay(contactPoint.point, newDirection, Color.green, 10);

            var lookRotation = Quaternion.LookRotation(newDirection);
            _rigidbody.rotation = lookRotation;
        }
    }
}