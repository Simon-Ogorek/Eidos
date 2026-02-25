using UnityEngine;
using System;

/// <summary>
/// Moves the player around using a Character Controller
/// </summary>
public class PlayerMovement : MonoBehaviour
{
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

    /// @brief
    [SerializeField]
    private float cameraRotationX = 1;
    [SerializeField]
    private float cameraRotationY = 1;

    void Start()
    {
        inputVector = Vector3.zero;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void FixedUpdate()
    {
        velocityVector *= friction;

        /* Change this out for the new input system */
        inputVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.z += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.z -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        velocityVector += inputVector * speed;

        Debug.DrawRay(transform.position, Vector3.down * transform.localScale.y * 1.1f, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 1.1f, Physics.DefaultRaycastLayers))
        {
            grounded = true;
            velocityVector.y = -0.1f;
            if (Input.GetKey(KeyCode.Space))
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

        controller.Move(velocityVector);

        /*
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, mouseX * cameraRotationY, 0));

        cameraTrackingPoints.transform.Rotate(new Vector3(-mouseX * cameraRotationX,0,0));

        */


     
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

       
        transform.Rotate(new Vector3(mouseY * cameraRotationY , 0, 0));
        
        transform.Rotate(new Vector3(0, mouseX * cameraRotationX, 0));
    
        

        

/*
        if (Input.GetKey(KeyCode.U))
        {
            transform.Rotate(new Vector3(1,0,0));

        }
*/
    }
}
