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
    bool isDashing;

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
    }

    void Update()
    {
        IsPlayerGrounded();
        //controller.Move(moveInput);
        Debug.DrawRay(transform.position, Vector3.forward * 10f, Color.blue);               
    }


    private void FixedUpdate()
    {
        myrigidbody.velocity = moveInput * moveSpeed;
        myrigidbody.AddForce(gravity * gravityScale, ForceMode.Acceleration);
        if (isJupming && isGrounded)
        {
            myrigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
        }
        if (isDashing)
        {
            myrigidbody.velocity = transform.forward * dashSpeed ;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = new Vector3 (context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
        if (moveInput != Vector3.zero) transform.forward = moveInput;
        //Debug.Log("Move input coordinates are: " + moveInput);
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
       // myrigidbody.velocity = new Vector3(jumpSpeed, myrigidbody.velocity.y + jumpSpeed, jumpSpeed);
        //controller.SimpleMove(new Vector3(0f, jumpSpeed, 0f));
        Debug.Log("Jump key pressed");
    }

    private void IsPlayerGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDashing = true;
        }
        else
        {
            isDashing = false;
        }
    }
}
