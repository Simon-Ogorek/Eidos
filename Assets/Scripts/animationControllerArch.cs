using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationControllerArch : MonoBehaviour
{

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isIdle", true);
    }

    // Update is called once per frame
    void Update()
    {
        }

    
}
