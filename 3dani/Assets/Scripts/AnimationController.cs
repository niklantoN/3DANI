using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AnimationController : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerHandler playerHandler;
    public bool _isStrafing;
    public Vector2 input;
    public bool _isGrounded;
    private bool _canDoubleJump;
    // Start is called before the first frame update
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = _characterController.isGrounded;
        animator.SetBool("IsGrounded", _isGrounded);
        if (_isGrounded)
        {
            _canDoubleJump = true;
        }

        if (!animator) return;
        var blend = input.sqrMagnitude;
        animator.SetBool("IsStrafing", _isStrafing);
        animator.SetFloat("Strafe.Y", input.y);
        animator.SetFloat("Strafe.X", input.x);
        animator.SetFloat("MovementCycle", blend);
        animator.SetFloat("VerticalVelocity", playerHandler.velocity);
    }


    public void Jump(InputAction.CallbackContext context)
    {

        if (!context.started) return;
        if (!_isGrounded && !_canDoubleJump) return;
        if (!_isGrounded && _canDoubleJump)
        {
            animator.SetTrigger("TrJump");
        }
        animator.SetTrigger("TrJump");
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

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
