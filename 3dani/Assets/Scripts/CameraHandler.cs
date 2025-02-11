using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    private Vector2 _input;
    private CameraRotation _cameraRotation;

    [SerializeField] private GameObject _camera;
    [SerializeField] private float _cameraSensivity;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private float _cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _camera.transform.eulerAngles = new Vector3(
            _camera.transform.eulerAngles.x + _input.y * Time.deltaTime * _cameraSensivity,
            _camera.transform.eulerAngles.y + _input.x * Time.deltaTime * _cameraSensivity,
            _camera.transform.eulerAngles.z
            );

        _camera.transform.position = _target.position - transform.forward * _distanceToTarget + new Vector3(0, _cameraHeight, 0);

    }

    public void Look(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }
}

public struct CameraRotation
{
    public float Pitch;
    public float Yaw;
}
