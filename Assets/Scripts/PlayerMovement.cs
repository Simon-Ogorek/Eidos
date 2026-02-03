


using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField]
    float speed = 0.05f;
    [SerializeField]
    float gravity = 0.1f;
    [SerializeField]
    float jumpForce = 0.2f;

    [Header("Constraint Values")]
    [SerializeField]
    float maxFallSpeed = 0.5f;
    [SerializeField]
    float maxWalkVelocity = 0.2f;
    [SerializeField]
    float maxSprintVelocity = 0.5f;
    
    private Vector3 inputVector;
    private Vector3 velocityVector;
    private CharacterController controller;
    private bool grounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputVector = Vector3.zero;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
    }
}
