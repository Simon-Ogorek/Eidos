using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : MonoBehaviour
{

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isSprinting = animator.GetBool("isSprinting");
        bool walkingPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool sprintingPressed = Input.GetKey(KeyCode.LeftShift);

        if(!isWalking && walkingPressed)
        {
            animator.SetBool("isWalking", true);
        }
        if(isWalking && !walkingPressed)
        {
            animator.SetBool("isWalking", false);
        }
        if(!isSprinting && (walkingPressed && sprintingPressed))
        {
            animator.SetBool("isSprinting", true);
        }
        if(isSprinting && (!walkingPressed || !sprintingPressed))
        {
            animator.SetBool("isSprinting", false);
        }

    }
}
