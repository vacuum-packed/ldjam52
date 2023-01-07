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

    private Vector2 _mousePosition;

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
        _mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(_mousePosition);
        if (Physics.Raycast(ray, out var raycastHit, 1000, LayerMask.GetMask("Floor")))
        {
            Debug.DrawRay(raycastHit.point, raycastHit.normal);
            Debug.Log("Raycasthit");
        }
        
    }
}
