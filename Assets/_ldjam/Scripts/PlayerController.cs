using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    Camera _camera;
    private GameObject _placeableObjectInstance;
    private Vector3 _lastValidPosition;

    void Start()
    {
        _camera = Camera.main;
        _placeableObjectInstance = Instantiate(placeableObjectPrefab);
        _placeableObjectInstance.transform.position = new Vector3(0,100,0);
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        _placeableObjectInstance.transform.position = _lastValidPosition;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var _mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(_mousePosition);
        if (Physics.Raycast(ray, out var raycastHit, 1000, LayerMask.GetMask("Floor")))
        {
            _lastValidPosition = raycastHit.point;
        }
        
    }
}
