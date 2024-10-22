using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private const string ISWALKING = "IsWalking";
    private const string ISGROUNDED = "IsGrounded";
    private const string ISJUMPING = "IsJumping";
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator.keepAnimatorStateOnDisable = true;
    }

    public void SetWalking(bool isWalking)
    {
        if (animator)
        {
            animator.SetBool(ISWALKING, isWalking);
        }
    }

    public void SetGrounded(bool isGrounded) 
    {
        if (animator)
        {
            animator.SetBool(ISGROUNDED, isGrounded);
        }
    }

    public void SetJumping(bool isJumping)
    {
        if (animator)
        {
            animator.SetBool(ISJUMPING, isJumping);
        }
    }

    public void FaceLeft(float movement)
    {
        if (animator && !Mathf.Approximately(movement, 0f)) 
        {
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = movement < 0 ? new Vector3(0f, 180f, 0f) : Vector3.zero;
            animator.transform.rotation = newRotation;
        }
    }
}
