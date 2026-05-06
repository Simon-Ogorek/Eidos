using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAnimationController : MonoBehaviour
{

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void SetIsWalking(bool cond)
    {
        animator.SetBool("IsWalking", cond);
        Debug.Log("SetIsWalking to " + cond);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("Triggered Attack");
    }
}
