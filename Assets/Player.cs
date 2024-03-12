using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody myrigidbody;
    Vector3 moveInput;
    private CharacterController controller;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float dashSpeed;

    bool isJupming;
    bool isGrounded;
    public bool isDashing;

    [SerializeField] float dashTimer;
    public float timer;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 gravity;
    [SerializeField] float gravityScale = 1f;

    void Start()
    {
        myrigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        gravity = new Vector3(1f, -9.81f, 1f);
        timer = dashTimer;
    }

    void Update()
    {
        IsPlayerGrounded();
        //controller.Move(moveInput);
        Debug.DrawRay(transform.position, Vector3.forward * 10f, Color.blue);
        if (timer < dashTimer)
        {
            timer += Time.deltaTime;
        }
        else isDashing = false;
    }


    private void FixedUpdate()
    {
        myrigidbody.AddForce(gravity * gravityScale, ForceMode.Acceleration);
        if (isJupming && isGrounded)
        {
            myrigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
        }
        if (isDashing && timer<dashTimer)
        {
            myrigidbody.velocity = transform.forward * dashSpeed;
        }        
        else myrigidbody.velocity = moveInput * moveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = new Vector3 (context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
        
        if (context.performed)
        {
            if (moveInput != Vector3.zero && !isDashing)
            {
                transform.forward = moveInput;
            }
        }
        
    }
    public void OnJump(InputAction.CallbackContext context) //for using keyboard
    {
        if (context.performed)
        {
            isJupming = true;
        }
        else
        {
            isJupming = false;
        }

    }

    private void IsPlayerGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed) { 
        isDashing = true;
        timer = 0f; }
    }


}
