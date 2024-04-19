using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    Rigidbody myrigidbody;
    Vector3 moveInput;

    [Header("Movement values")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    [Header("Other")]
    [SerializeField] private int lives;
    [SerializeField] GameObject throwDirectionIndicator;

    bool isJumping;
    bool isGrounded;
    public bool isThrowing;

    [Header("Dashing parameters")]
    bool isDashing;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTimer;
    [SerializeField] float dashCooldown;
    public float timer;
    public float timer2;

    [Header("Ground checking")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    [Header("Gravity")]
    Vector3 gravity;
    [SerializeField] float gravityScale = 1f;

    private void Awake()
    {
        InputUser.PerformPairingWithDevice(InputSystem.GetDevice("Keyboard")); //assign the player to the keyboard - this makes it possible to play 2 players with the keyboard simultaniously
    }
    void Start()
    {
        // var instance = PlayerInput.Instantiate(input.gameObject, controlScheme: "Keyboard&Mouse", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });

        throwDirectionIndicator.SetActive(false);

        PlayerInput playerInput = GetComponent<PlayerInput>();
        Debug.Log(playerInput.currentActionMap.name);

        myrigidbody = GetComponent<Rigidbody>();

        gravity = new Vector3(1f, -9.81f, 1f);
        timer = dashTimer;
        timer = dashCooldown;
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
        if (timer2 < dashCooldown)
        {
            timer2 += Time.deltaTime;
        }
    }


    private void FixedUpdate()
    {
       // myrigidbody.AddForce(gravity * gravityScale, ForceMode.Acceleration);
        if (isJumping && isGrounded)
        {
            myrigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
        }
        if (isDashing && timer < dashTimer)
        {
            myrigidbody.velocity = transform.forward * dashSpeed;
        }
        else if (!isThrowing) myrigidbody.velocity = moveInput * moveSpeed;
        else myrigidbody.velocity = Vector3.zero;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = new Vector3 (context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
               
        if (context.performed)
        {
            if (moveInput != Vector3.zero /*&& !isDashing*/) //commented this, so turning during dash is possible (if dashing constantly)
            {
                transform.forward = moveInput;
            }
        }
        
    }
    public void OnJump(InputAction.CallbackContext context) //for using keyboard
    {
        if (context.performed)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

    }

    private void IsPlayerGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && timer2 >= dashCooldown) { 
        isDashing = true;
        timer = 0f;
        timer2 = 0f;
        }
    }

    public void GetDamaged(int damage)
    {
        lives -= damage;
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {

        if (GetComponentInChildren<RangedAttack>())
        {
            isThrowing = true;
            throwDirectionIndicator.SetActive(true);
        }

        if (context.canceled)
        {
            isThrowing = false;
            throwDirectionIndicator.SetActive(false);
        }
    }
}
