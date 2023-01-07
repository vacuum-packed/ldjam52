using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    Camera _camera;
    private GameObject _placebleObjectInstance;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        
        mousePosition = _camera.ScreenToWorldPoint(mousePosition);
        var ray = new Ray(mousePosition, Vector3.down);
        if (Physics.Raycast(ray, out var raycastHit, Single.PositiveInfinity, LayerMask.NameToLayer("Floor")))
        {
        }
        
    }
}
