using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Moves the player around using a Character Controller
/// </summary>
public class PlayerMovement : MonoBehaviour
{

    /// @brief to match direction character goes relative to camera
    public enum Direction
    {
        Forward,
        Backward,
        Right,
        Left
    }

    [Header("Movement Values")]

    /// @brief How fast the player moves in X and Z
    [SerializeField]
    float speed = 0.05f;

    /// @brief Applied every frame where the player isnt grounded
    [SerializeField]
    float gravity = 0.1f;

    /// @brief Force applied on jump
    [SerializeField]
    float jumpForce = 0.2f;

    /// @brief Multiplied against velocity every frame (0 < f < 1)
    [SerializeField, Range(0,1)]
    float friction = 0.9f;

    [Header("Constraint Values")]

    /// @brief How fast y velocity can be (+-)
    [SerializeField]
    float maxFallSpeed = 0.5f;

    /// @brief How fast x and y velocity can be by default (+-)
    [SerializeField]
    float maxWalkVelocity = 0.2f;

    /// @brief | Not Implemented | How fast x and y velocity can be if sprinting (+-)
    [SerializeField, Obsolete("Not implemented yet")]
    float maxSprintVelocity = 0.5f;
    
    /// @brief What the player inputted, resets every frame
    private Vector3 inputVector;

    /// @brief How fast the player is
    private Vector3 velocityVector;

    private CharacterController controller;

    /// @brief Is the player grounded
    private bool grounded;

    /// @brief Camera tracking points to be rotated accoding to player input
    [SerializeField]
    private GameObject cameraTrackingPoints;

    /// @brief freeLookCamera that affects movement direction based on rotation.
    [SerializeField]
    private GameObject freeLookCamera;
    
    /// @brief player direction to adjust relative to camera.
    private Direction playerDirection;
    /// @brief
    [SerializeField]
    private float cameraRotationX = 5;
    [SerializeField]
    private float cameraRotationY = 5;

    void Start()
    {
        inputVector = Vector3.zero;
        controller = GetComponent<CharacterController>();
        //for camera control
        Cursor.lockState = CursorLockMode.Locked;
        playerDirection = Direction.Forward;
    }

    void FixedUpdate()
    {
        velocityVector *= friction;
        /* Change this out for the new input system */
        inputVector = Vector3.zero;

        if(controllerMove())
        {
            inputVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W))
        {
            matchRotation(Direction.Forward);
            inputVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            matchRotation(Direction.Left);
            inputVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            matchRotation(Direction.Backward);
            inputVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            matchRotation(Direction.Right);
            inputVector += transform.forward;
        }

        inputVector = Vector3.Normalize(inputVector);
        
        velocityVector += inputVector * speed;

        Debug.DrawRay(transform.position, Vector3.down * transform.localScale.y * 1.1f, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 1.1f, Physics.DefaultRaycastLayers))
        {
            grounded = true;
            velocityVector.y = -0.1f;
            if (Input.GetKey(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed)
            {
                velocityVector.y += jumpForce;
            }
        }
        else
        {
            grounded = false;
            velocityVector.y -= gravity;
        }

        Debug.Log(grounded);

        Mathf.Clamp(velocityVector.x,-1*maxWalkVelocity,maxWalkVelocity);
        Mathf.Clamp(velocityVector.z,-1*maxWalkVelocity,maxWalkVelocity);
        Mathf.Clamp(velocityVector.y,-1*maxFallSpeed,maxFallSpeed);

        controller.Move(velocityVector * Time.deltaTime);

        /*
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, mouseX * cameraRotationY, 0));

        cameraTrackingPoints.transform.Rotate(new Vector3(-mouseX * cameraRotationX,0,0));

        */
    }

    /// @brief maches the rotation of the player relative to the rotation of the camera
    public void matchRotation(Direction playerMotion)
    {
        Vector3 playerRotation = transform.eulerAngles;
        Vector3 cameraRotation = freeLookCamera.transform.eulerAngles;
        Vector3 newPlayerRotation = new Vector3(playerRotation.x, cameraRotation.y, cameraRotation.z);
        if(playerMotion == Direction.Forward)
            newPlayerRotation = new Vector3(playerRotation.x, cameraRotation.y, cameraRotation.z);
        else if (playerMotion == Direction.Backward)
            newPlayerRotation = new Vector3(playerRotation.x, cameraRotation.y + 180, cameraRotation.z);
        else if (playerMotion == Direction.Right)
            newPlayerRotation = new Vector3(playerRotation.x, cameraRotation.y + 90, cameraRotation.z);    
        else if (playerMotion == Direction.Left)
            newPlayerRotation = new Vector3(playerRotation.x, cameraRotation.y - 90, cameraRotation.z);    
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newPlayerRotation), Time.deltaTime * 5);
    }

    /// @brief gives support for controller movement
    public bool controllerMove()
    {
        Gamepad controller = Gamepad.current;
        if(controller.leftStick.ReadValue() != new Vector2(0,0))
        {
            Vector2 leftStick = controller.leftStick.ReadValue();
            if(leftStick.y > 0 && leftStick.x < 0.1 && leftStick.x > -0.1)
                matchRotation(Direction.Forward);
            else if(leftStick.y > 0 && leftStick.x < 0.9 && leftStick.x >= 0.1){
                matchRotation(Direction.Forward);
                matchRotation(Direction.Right);
            }
            else if(leftStick.y > 0 && leftStick.x > -0.9 && leftStick.x <= -0.1){
                matchRotation(Direction.Forward);
                matchRotation(Direction.Left);
            }
            else if(leftStick.y < 0 && leftStick.x < 0.1 && leftStick.x > -0.1)
                matchRotation(Direction.Backward);
            else if(leftStick.y < 0 && leftStick.x < 0.9 && leftStick.x >= 0.1){
                matchRotation(Direction.Backward);
                matchRotation(Direction.Right);
            }
            else if(leftStick.y < 0 && leftStick.x > -0.9 && leftStick.x <= -0.1){
                matchRotation(Direction.Backward);
                matchRotation(Direction.Left);
            }
            else if(leftStick.y > -0.25 && leftStick.y < 0.1 && leftStick.x < 0)
                matchRotation(Direction.Left);
            else if(leftStick.y > -0.25 && leftStick.y < 0.1 && leftStick.x > 0)
                matchRotation(Direction.Right);
            return true;
        }
        return false;
    }


}
