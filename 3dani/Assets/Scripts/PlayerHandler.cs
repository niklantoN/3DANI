using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerHandler : MonoBehaviour
{
    public Vector2 input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private float _velocity;
    private bool _isGrounded;

    private LayerMask _groundLayer;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _jumpStrength;
    [SerializeField] private Transform _camera;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;



        ApplyGravity();
        RotateCharacter();
        MoveCharacter();
    }








    private void RotateCharacter()
    {
        if (input.sqrMagnitude == 0) return;
        
            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle + _camera.eulerAngles.y, 0);
        
    }

    private void MoveCharacter()
    {
        Vector3 movementInCameraSpace = ConvertToCameraSpace(_direction);
        _characterController.Move(movementInCameraSpace * Time.deltaTime * _movementSpeed);
    }

    private void ApplyGravity()
    {
        if (_isGrounded && _velocity < 0.0f)
        {
            _velocity = -1;
        }
        else
        {
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
    }


    public void Jump(InputAction.CallbackContext context)
    {

            if (!context.started) return;
            if(!_isGrounded) return;
            _velocity += _jumpStrength;
    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        if (input.magnitude > 1)
        {
            input.Normalize();
        }
        _direction = new Vector3(input.x, 0, input.y);
    }

    Vector3 ConvertToCameraSpace(Vector3 direction)
    {
        Quaternion rotation = Quaternion.Euler(0, _camera.eulerAngles.y, 0);

        Vector3 rotatedVector = rotation * direction;
        return rotatedVector;
    }

}
