using System;
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
    public float velocity;
    private bool _isGrounded;
    private bool _canDoubleJump;
    public bool _isStrafing;

    public float totalTime = 0.3f;
    public float moveDuration = 0.1f;
    public float dashDistance;

    private float timer = 0f;
    private bool isDashing = false;
    private Vector3 dashDirection;

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
        
        if (_isGrounded)
        {
            _canDoubleJump = true;
        }

        if (isDashing)
        {
            timer += Time.deltaTime;

            if (timer <= moveDuration)
            {
                _characterController.Move(transform.forward * 10 * dashDistance * Time.deltaTime);
            }

            if (timer >= totalTime)
            {
                isDashing = false;
            }
        }

        ApplyGravity();
        RotateCharacter();
        MoveCharacter();
    }







    public void Strafe(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isStrafing = true;
        }

        if (context.canceled)
        {
            _isStrafing = false;
        }
        return;
    }
    private void RotateCharacter()
    {
        if (isDashing) return;
        if (_isStrafing)
        {
            transform.rotation = Quaternion.Euler(0, _camera.eulerAngles.y, 0);
        }
        if (input.sqrMagnitude == 0) return;
        if (!_isStrafing)
        {
            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle + _camera.eulerAngles.y, 0);
        }
        
    }

    private void MoveCharacter()
    {
        if (isDashing) return;
        Vector3 movementInCameraSpace = ConvertToCameraSpace(_direction);
        _characterController.Move(movementInCameraSpace * Time.deltaTime * _movementSpeed);
    }

    private void ApplyGravity()
    {
        if (_isGrounded && velocity < 0.0f)
        {
            velocity = -1;
        }
        else
        {
            velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        _direction.y = velocity;
    }


    public void Jump(InputAction.CallbackContext context)
    {

            if (!context.started) return;
            if (isDashing) return;
            if (!_isGrounded && !_canDoubleJump) return;
            if (!_isGrounded && _canDoubleJump)
            {
            velocity = _jumpStrength * 0.75f;
            _canDoubleJump = false;
            return;
            }
            velocity += _jumpStrength;
    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        _direction = new Vector3(input.x, 0, input.y);
    }

    Vector3 ConvertToCameraSpace(Vector3 direction)
    {
        Quaternion rotation = Quaternion.Euler(0, _camera.eulerAngles.y, 0);

        Vector3 rotatedVector = rotation * direction;
        return rotatedVector;
    }

    public void Melee(InputAction.CallbackContext context)
    {
        if(_isGrounded)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        timer = 0f;
        dashDirection = _direction; // Moves in the forward direction of the object
    }

}
