using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private const string ISWALKING = "IsWalking";
    private const string ISGROUNDED = "IsGrounded";
    private const string ISJUMPING = "IsJumping";
    private const string SPEEDMULTIPLIER = "SpeedMultiplier";
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera leftCamera;
    [SerializeField] private CinemachineVirtualCamera rightCamera;

    private Flashlight flashlight;

    private void Awake()
    {
        animator.keepAnimatorStateOnDisable = true;
        flashlight = GetComponent<Flashlight>();
        leftCamera.Priority = 5;
        rightCamera.Priority = 10;
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

    public void SetFaceDirection(float movement)
    {
        if (animator)
        {
            Quaternion newRotation = animator.transform.rotation;
            Vector3 eulerAngles = newRotation.eulerAngles;
            bool moving = !Mathf.Approximately(movement, 0f);
            animator.SetFloat(SPEEDMULTIPLIER, 1f);
            if (moving)
            {
                eulerAngles = movement < 0 ? new Vector3(0f, 180f, 0f) : Vector3.zero;
                leftCamera.Priority = movement < 0 ? 10 : 5;
                rightCamera.Priority = movement < 0 ? 5 : 10;
            }
            if (flashlight.FlashlightActive)
            {
                if (flashlight.FlashlightDot < 0)
                {
                    eulerAngles = new Vector3(0f, 180f, 0f);
                    if (movement < 0) animator.SetFloat(SPEEDMULTIPLIER, 1f);
                    else animator.SetFloat(SPEEDMULTIPLIER, -1f);
                } else
                {
                    eulerAngles = Vector3.zero;
                    if (movement > 0) animator.SetFloat(SPEEDMULTIPLIER, 1f);
                    else animator.SetFloat(SPEEDMULTIPLIER, -1f);
                }
            }
            
            newRotation.eulerAngles = eulerAngles;
            animator.transform.rotation = newRotation;
        }
    }
}
