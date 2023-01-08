using _ldjam.Scripts.ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    private enum Foot
    {
        None,
        Left,
        Right
    }

    [SerializeField]
    private GameObject leftFoot;

    [SerializeField]
    private GameObject rightFoot;

    [SerializeField]
    private float maxFoodDistance;

    private float _maxFoodDistanceSqr;

    [SerializeField]
    private GameObject indicatorGameObject;

    [SerializeField]
    private VisualEffect effect;

    [Header("Broadcasting Events")]
    [SerializeField]
    private BoolEventChannelSO isPositionValidEvent;


    private GameObject _lastFootPlaced;
    private GameObject _currentFootToPlace;
    private Foot _currentPlaceFoot;
    private Camera _camera;
    private Vector3 _lastPosition;
    private bool _isPositionValid;
    private int _layer;
    private readonly Vector3 _outOfBoundsPosition = new(0, 100, 0);

    private void Start()
    {
        _camera = Camera.main;
        _layer = LayerMask.NameToLayer("Floor");
        _currentPlaceFoot = Foot.Left;
        leftFoot.transform.position = Vector3.zero;
        rightFoot.transform.position = _outOfBoundsPosition;
        _lastFootPlaced = leftFoot;
        _maxFoodDistanceSqr = maxFoodDistance * maxFoodDistance;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.action.phase != InputActionPhase.Canceled || !_isPositionValid) return;
        switch (_currentPlaceFoot)
        {
            case Foot.None:
                leftFoot.transform.position = _lastPosition;
                rightFoot.transform.position = _outOfBoundsPosition;
                _lastFootPlaced = leftFoot;
                _currentPlaceFoot = Foot.Left;
                break;
            case Foot.Left:
                rightFoot.transform.position = _lastPosition;
                leftFoot.transform.position = _outOfBoundsPosition;
                _lastFootPlaced = rightFoot;
                _currentPlaceFoot = Foot.Right;
                break;
            case Foot.Right:
                leftFoot.transform.position = _lastPosition;
                rightFoot.transform.position = _outOfBoundsPosition;
                _lastFootPlaced = leftFoot;
                _currentPlaceFoot = Foot.Left;
                break;
        }

        effect.Play();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var raycastHit, 10000))
        {
            _lastPosition = raycastHit.point;
            indicatorGameObject.transform.position = _lastPosition;
            if (raycastHit.transform.gameObject.layer == _layer && IsInRadius())
            {
                _isPositionValid = true;
                isPositionValidEvent.RaiseEvent(true);
            }
            else
            {
                _isPositionValid = false;
                isPositionValidEvent.RaiseEvent(false);
            }
        }
        else
        {
            _lastPosition = _outOfBoundsPosition;
        }
    }

    private bool IsInRadius()
    {
        return (_lastFootPlaced.transform.position - _lastPosition).sqrMagnitude <= _maxFoodDistanceSqr;
    }
}
