using _ldjam.Scripts.ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform leftFoot;

    [SerializeField]
    private Transform rightFoot;

    [SerializeField]
    private Transform indicator;

    [SerializeField]
    private Transform radiusIndicator;

    [SerializeField]
    private float startFootDistance = .1f;

    [SerializeField]
    private float maxFootDistance = .762f;

    [SerializeField]
    private float scrollSensitivity = 6;

    [SerializeField]
    private VisualEffect effect;

    [Header("Broadcasting Events")]
    [SerializeField]
    private BoolEventChannelSO isPositionValidEvent;

    private Transform _lastFootPlaced;
    private bool _leftFootPlaced = true;
    private float _maxFoodDistanceSqr;
    private Camera _camera;
    private bool _isPositionValid;
    private int _layer;
    private readonly Vector3 _outOfBoundsPosition = new(0, 100, 0);

    private void Start()
    {
        _camera = Camera.main;
        _layer = LayerMask.NameToLayer("Floor");
        leftFoot.localPosition = Vector3.right * -startFootDistance;
        rightFoot.localPosition = Vector3.right * startFootDistance;
        _maxFoodDistanceSqr = maxFootDistance * maxFootDistance;
    }

    private void Update()
    {
        float yRot = indicator.localEulerAngles.y;
        yRot += Input.mouseScrollDelta.y * scrollSensitivity;
        indicator.localRotation = Quaternion.AngleAxis(yRot, Vector3.up);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.action.phase != InputActionPhase.Canceled || !_isPositionValid)
            return;

        _lastFootPlaced = _leftFootPlaced ? rightFoot : leftFoot;
        _lastFootPlaced.SetPositionAndRotation(indicator.position, indicator.rotation);
        Vector3 avgPos = (leftFoot.localPosition + rightFoot.localPosition) * .5f;
        avgPos.y = radiusIndicator.localPosition.y;
        radiusIndicator.localPosition = avgPos;

        effect.Play();
        _leftFootPlaced = !_leftFootPlaced;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out var raycastHit, 10000))
            return;

        Vector3 hitPosition = raycastHit.point;
        indicator.position = hitPosition;

        bool valid = raycastHit.transform.gameObject.layer == _layer && IsInRadius(hitPosition);
        _isPositionValid = valid;
        isPositionValidEvent.RaiseEvent(valid);
    }

    private bool IsInRadius(Vector3 position)
    {
        return (radiusIndicator.position - position).sqrMagnitude <= _maxFoodDistanceSqr;
    }
}
