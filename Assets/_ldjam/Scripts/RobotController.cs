using UnityEngine;
using Random = UnityEngine.Random;

namespace _ldjam.Scripts
{
    public class RobotController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 startPosition;

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private float maxVelocity;

        private Rigidbody _rigidbody;

        private bool _isCollided;

        [Header("Listening Events")]
        [SerializeField]
        private VoidEventChannelSO onGameStarted;

        [SerializeField]
        private VoidEventChannelSO onGameEnded;

        private bool _running;

        private void OnEnable()
        {
            onGameStarted.onEventRaised += OnGameStarted;
            onGameEnded.onEventRaised += OnGameEnded;
        }

        private void OnGameStarted()
        {
            InitRobot();
            _running = true;
        }

        private void OnGameEnded()
        {
            _running = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void InitRobot()
        {
            transform.position = startPosition;
            var angle = Random.Range(0, 360);
            Debug.Log(angle);
            var euler = new Vector3(0, angle, 0);
            var newRotation = new Quaternion();
            newRotation.eulerAngles = euler;
            _rigidbody.rotation = newRotation;
        }

        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position, transform.forward * .5f);
            Debug.DrawRay(transform.position, transform.up * .5f);

            if (!_running) return;

            if (_isCollided) return;


            if (_rigidbody.velocity.sqrMagnitude <= maxVelocity * maxVelocity)
            {
                _rigidbody.AddForce(transform.forward * acceleration);
            }
            else
            {
                _rigidbody.velocity.Normalize();
            }
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