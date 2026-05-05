using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using System.Collections;


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

    /// @brief Player is in dialogue
    public bool cantMove = false;

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

    bool usingController = false;
    float defaultSpeed;
    Coroutine castingCoroutine;
    bool coroutineRunning = false;

    public bool canMove = true;

    bool sprinting = false;

    void Start()
    {
        defaultSpeed = speed;
        inputVector = Vector3.zero;
        controller = GetComponent<CharacterController>();
        //for camera control
        Cursor.lockState = CursorLockMode.Locked;
        playerDirection = Direction.Forward;
    }

    void Update()
    {
        velocityVector *= friction;

        if (cantMove || !canMove)
            return;

        usingController = Gamepad.current != null;
        
        Vector3 movemntDir = GetCameraOrientedInput(usingController);
        velocityVector += movemntDir * speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 1.1f, Physics.DefaultRaycastLayers))
        {
            grounded = true;
            velocityVector.y = -0.1f;
            if (Input.GetKey(KeyCode.Space) || (usingController && Gamepad.current.buttonSouth.isPressed))
                velocityVector.y += jumpForce;
        }
        else
        {
            grounded = false;
            velocityVector.y -= gravity;
        }

        Mathf.Clamp(velocityVector.x,-1*maxWalkVelocity,maxWalkVelocity);
        Mathf.Clamp(velocityVector.z,-1*maxWalkVelocity,maxWalkVelocity);
        Mathf.Clamp(velocityVector.y,-1*maxFallSpeed,maxFallSpeed);

        controller.Move(velocityVector);
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        cameraTrackingPoints.transform.Rotate(new Vector3(-mouseX * cameraRotationX,0,0));
        Debug.Log(velocityVector);
        Vector3 XYDir = new Vector3(velocityVector.x, 0, velocityVector.z);
        if (XYDir.sqrMagnitude >= 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(XYDir), Time.deltaTime * 10);
    }
    /// @brief returns the input vector in respect to the camera

    Vector3 GetCameraOrientedInput(bool usingController)
    {
        if (usingController)
        {
            Gamepad controller = Gamepad.current;
            Vector3 inputDir = controller.leftStick.ReadValue();

            inputDir = freeLookCamera.transform.TransformDirection(inputDir.normalized);

            return inputDir;
        }
        else // using keyboard
        {
            Vector3 inputDir = Vector3.zero;

            inputDir.z += Input.GetKey(KeyCode.W) ? 1 : 0;
            inputDir.z += Input.GetKey(KeyCode.S) ? -1 : 0;
            inputDir.x += Input.GetKey(KeyCode.A) ? -1 : 0;
            inputDir.x += Input.GetKey(KeyCode.D) ? 1 : 0;
            
            inputDir = freeLookCamera.transform.TransformDirection(inputDir.normalized);
            
            return inputDir;
        }   
    }
    /*
    void FixedUpdate()
    {
        sprinting = Input.GetKey(KeyCode.LeftShift);
        velocityVector *= friction;
        // Change this out for the new input system 
        inputVector = Vector3.zero;

        if (!canMove)
            return;

        //Only do controller movement if controller is connected.
        if(Gamepad.current!=null){
            usingController = true;
        }
        else if(Gamepad.current==null)
        {
            usingController = false;
        }

        if(usingController && controllerMove() && !cantMove)
        {
            inputVector += transform.forward;
        }

        if(!cantMove){
        bool w = Input.GetKey(KeyCode.W);
        bool s = Input.GetKey(KeyCode.S);
        bool a = Input.GetKey(KeyCode.A);
        bool d = Input.GetKey(KeyCode.D);

        if (w && !s)
        {
            matchRotation(Direction.Forward);
            inputVector += transform.forward;
        }

        else if (s && !w)
        {
            matchRotation(Direction.Backward);
            inputVector += transform.forward;
        }

        if (a && !d)
        {
            matchRotation(Direction.Left);
            inputVector += transform.forward;
        }


        if (d && !a)
        {
            matchRotation(Direction.Right);
            inputVector += transform.forward;
        }
        }

        inputVector = Vector3.Normalize(inputVector);
        
        if(!sprinting)
            velocityVector += inputVector * speed;
        else
            velocityVector += inputVector * speed * 5;

        Debug.DrawRay(transform.position, Vector3.down * transform.localScale.y * 1.1f, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 1.1f, Physics.DefaultRaycastLayers))
        {
            grounded = true;
            velocityVector.y = -0.1f;
            if (Input.GetKey(KeyCode.Space) || (usingController && Gamepad.current.buttonSouth.isPressed))
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

        float maxVelocity = sprinting ? maxSprintVelocity : maxWalkVelocity;

        //Clamp horizontal velocity together so it feels better and doesn't sum up.
        Vector3 horizontalVelocity = new Vector3(velocityVector.x, 0, velocityVector.z);

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxVelocity);

        velocityVector.x = horizontalVelocity.x;
        velocityVector.z = horizontalVelocity.z;

        if(!grounded)
            velocityVector.y = Mathf.Clamp(velocityVector.y,-1*maxFallSpeed,maxFallSpeed);

        controller.Move(velocityVector * Time.deltaTime);

        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, mouseX * cameraRotationY, 0));

        cameraTrackingPoints.transform.Rotate(new Vector3(-mouseX * cameraRotationX,0,0));

        
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
    }*/
    

    IEnumerator CastMovement(float time)
    {
        speed = defaultSpeed * 0.1f;
        Debug.Log($"Cast Movement started {speed} for {name}");
        yield return new WaitForSeconds(time);
        speed = defaultSpeed;
        Debug.Log($"Cast Movement ended {speed} for {name}");


    }
    public void StartCastMovement(float time)
    {
        if (coroutineRunning)
        {
            EndCastMovement();
        }
        castingCoroutine = StartCoroutine(CastMovement(time));
    }
    public void EndCastMovement()
    {
        if (coroutineRunning)
        {
            StopCoroutine(castingCoroutine);
        }
    }
}
