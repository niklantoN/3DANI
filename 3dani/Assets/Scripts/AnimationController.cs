using System;
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
    public LayerMask groundLayer;
    public LayerMask playermask;
    public float footOffset = 0.1f;
    [Range(0f, 1f)]
    public float ikdistance;

    public float totalTime = 0.3f;

    private float timer = 0f;

    private bool isDashing = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            timer += Time.deltaTime;

            if (timer >= totalTime)
            {
                isDashing = false;
            }
        }


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


    public void Melee(InputAction.CallbackContext context)
    {
        if (isDashing) return;
        if (_isGrounded)
        {
            StartDash();
            isDashing = true;
        }
    }

    void StartDash()
    {
        isDashing = true;
        timer = 0f;
        animator.SetTrigger("TrMelee");
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

        RaycastHit hitL;
        Ray rayL = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(rayL, out hitL, ikdistance + 1f, playermask))
        {

            if (hitL.transform.tag == "Walkable")
            {

                Vector3 footPosition = hitL.point;
                footPosition.y += ikdistance;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            }

        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

        RaycastHit hitR;
        Ray rayR = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(rayR, out hitR, ikdistance + 1f, playermask))
        {

            if (hitR.transform.tag == "Walkable")
            {

                Vector3 footPosition = hitR.point;
                footPosition.y += ikdistance;
                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            }

        }

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
