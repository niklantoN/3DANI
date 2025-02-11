using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerHandler PlayerHandler;
    // Start is called before the first frame update
    void Start()
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator) return;
        var blend = PlayerHandler.input.sqrMagnitude;
        animator.SetFloat("MovementCycle", blend);
    }
}
