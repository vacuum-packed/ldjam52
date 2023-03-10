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
    private float startFootDistance = .1f;

    [SerializeField]
    private float maxFootDistance = .762f;

    [SerializeField]
    private float scrollSensitivity = 6;

    [SerializeField]
    private VisualEffect effect;

    private bool _leftFootPlaced;
    private Camera _camera;
    private bool _isOnLayer;
    private int _layer;
    private bool _scrolled;

    [Header("Listening Events")]
    [SerializeField]
    private VoidEventChannelSO roundStartedEvent;

    [Header("Broadcasting Events")]
    [SerializeField]
    private BoolEventChannelSO isPositionValidEvent;

    private AudioSource _audioSource;

    private void OnEnable()
    {
        roundStartedEvent.onEventRaised += OnRoundStarted;
    }

    private void OnDisable()
    {
        roundStartedEvent.onEventRaised -= OnRoundStarted;
    }

    private void Start()
    {
        _camera = Camera.main;
        _layer = LayerMask.NameToLayer("Floor");
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y == 0)
            return;

        _scrolled = true;
        float yRot = indicator.localEulerAngles.y;
        yRot += Input.mouseScrollDelta.y * scrollSensitivity;
        indicator.localRotation = Quaternion.AngleAxis(yRot, Vector3.up);
    }

    private void OnRoundStarted()
    {
        leftFoot.localPosition = Vector3.right * -startFootDistance;
        leftFoot.localRotation = Quaternion.identity;
        rightFoot.localPosition = Vector3.right * startFootDistance;
        rightFoot.localRotation = Quaternion.identity;
        indicator.localScale = Vector3.one;
        _leftFootPlaced = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.action.phase != InputActionPhase.Canceled || !_isOnLayer)
            return;

        // Handle foot
        Transform foot = _leftFootPlaced ? rightFoot : leftFoot;
        foot.localPosition = indicator.localPosition;
        foot.localRotation = indicator.localRotation;

        Vector3 indicatorScale = indicator.localScale;
        indicatorScale.x = -indicatorScale.x;
        indicator.localScale = indicatorScale;

        // Handle vfx
        effect.transform.localPosition = indicator.localPosition;
        effect.Play();
        _audioSource.Play();

        _scrolled = false;
        _leftFootPlaced = !_leftFootPlaced;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out var raycastHit, 10000))
            return;

        // Clamp strid
        Vector3 center = (leftFoot.position + rightFoot.position) * .5f;
        Vector3 hitPosition = raycastHit.point;
        Vector3 lookDir = raycastHit.point - center;
        Vector3 stride = Vector3.ClampMagnitude(lookDir, maxFootDistance);
        Vector3 footPosition = center + stride;

        // Raycast again with clamped position
        ray = _camera.ScreenPointToRay(_camera.WorldToScreenPoint(footPosition));
        Physics.Raycast(ray, out raycastHit, 10000);

        indicator.position = footPosition;
        if (!_scrolled)
            indicator.localRotation = Quaternion.LookRotation(lookDir, Vector3.up);

        _isOnLayer = raycastHit.transform.gameObject.layer == _layer;
        isPositionValidEvent.RaiseEvent(_isOnLayer);
    }
}
