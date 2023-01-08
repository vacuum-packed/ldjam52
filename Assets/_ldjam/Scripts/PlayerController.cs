using _ldjam.Scripts.ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    [SerializeField]
    private GameObject indicatorGameObject;

    [Header("Broadcasting Events")]
    [SerializeField]
    private BoolEventChannelSO isPositionValidEvent;

    Camera _camera;
    private GameObject _placeableObjectInstance;
    private Vector3 _lastPosition;
    private Vector3 _lastValidPosition;
    private int _layer;

    void Start()
    {
        _camera = Camera.main;
        _layer = LayerMask.NameToLayer("Floor");
        _placeableObjectInstance = Instantiate(placeableObjectPrefab);
        _placeableObjectInstance.transform.position = new Vector3(0, 100, 0);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.action.phase != InputActionPhase.Canceled) return;
        _placeableObjectInstance.transform.position = _lastValidPosition;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var raycastHit, 10000))
        {
            Debug.Log($"Layer: {LayerMask.LayerToName(raycastHit.transform.gameObject.layer)}");
            _lastPosition = raycastHit.point;
            indicatorGameObject.transform.position = _lastPosition;
            if (raycastHit.transform.gameObject.layer == _layer)
            {
                _lastValidPosition = _lastPosition;
                isPositionValidEvent.RaiseEvent(true);
            }
            else
            {
                _lastValidPosition = new Vector3(0, 100, 0);
                isPositionValidEvent.RaiseEvent(false);
            }
        }
        else
        {
            _lastPosition = new Vector3(0, 100, 0);
        }
    }
}