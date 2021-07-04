#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public myColyseusClient myClient; 
    public float speed = 12f;
    public float gravity = -10f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    

    Vector3 velocity;
    bool isGrounded;

#if ENABLE_INPUT_SYSTEM
    InputAction movement;

    void Start()
    {
        movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        movement.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        movement.Enable();
    }
#endif

    // Update is called once per frame
    void FixedUpdate()
    {
        float x;
        float y;

#if ENABLE_INPUT_SYSTEM
        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        y = delta.y;
#else
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
#endif
        Vector3 move = transform.right * x + transform.forward * y;

        if(move != Vector3.zero) 
            {
                controller.Move(move * speed * Time.deltaTime);
                myClient.OnTankMove();
            }

        controller.Move(velocity * Time.deltaTime);
    }
}
