using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 inputVector;
    [SerializeField]
    int jumpForce = 50,
    playerSpeed = 200,
    maxSpeed = 4;
    [SerializeField]
    private LayerMask groundLayer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        inputVector = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    
    void FixedUpdate()
    {   
        Move();
        handleAnimations();
        flipTowardsMovement();
        isGrounded();
        
    }

    // Hint: rb.AddForce(forceVector, ForceMode2D.Impulse);
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            if (isGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }


    public void OnMoveInput(InputAction.CallbackContext context) {
        inputVector = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (isGrounded())
        {
            rb.AddForce(new(inputVector.x * playerSpeed, 0));
            rb.velocity = new(Math.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
        }
    }

    private bool isGrounded() {
        float heightControlParam = 0.2f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center,capsuleCollider.bounds.size, 0, Vector2.down, heightControlParam, groundLayer);
        return raycastHit.collider != null;
    }

    private void flipTowardsMovement()
    {
        var sign = Math.Sign(inputVector.x);
        if (sign != 0)
        {
            transform.localScale = new(sign, 1, 1);
        }
    }

    private void handleAnimations() {
        animator.SetBool("Running", Math.Abs(rb.velocity.x) > float.Epsilon);
        animator.SetBool("Falling", !isGrounded());
    }
}
