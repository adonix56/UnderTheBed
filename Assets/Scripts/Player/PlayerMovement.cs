using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private int jumpMaxCount;
    [SerializeField] private float groundCheckDelay;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask platformMask;

    private float lastGroundedTime;
    private float jumpDelay;
    private bool isGrounded;
    private bool isRotating;
    private bool isJumping;
    private bool canMove = true;
    private int jumpNum;
    private float movement;
    [SerializeField] private Vector3 axisOfMovement;
    private Vector3 startAxis;
    private Vector3 endAxis;
    private float distanceAxis;
    private float maxDistanceAxis;
    private float minClamp;
    private float maxClamp;
    private float targetYRotation;
    private PlayerController playerController;
    private PlayerAnimation playerAnimation;
    private Rigidbody rb;

    private void Awake()
    {
        //TEST
        axisOfMovement = new Vector3(1f, 0f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.JumpPressed += Jump;
        playerAnimation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movement = playerController.GetMoveInput();
            isGrounded = Physics.CheckBox(transform.position, boxSize, transform.rotation, platformMask);
            playerAnimation.SetWalking(!Mathf.Approximately(movement, 0f));
            playerAnimation.SetFaceDirection(movement);
            if (isJumping)
            {
                if (jumpDelay < 0f)
                {
                    isJumping = false;
                    playerAnimation.SetJumping(false);
                }
                jumpDelay -= Time.deltaTime;
            } else
            {
                if (isGrounded && lastGroundedTime <= 0f) // Grounded after delay
                {
                    //playerAnimation.SetGrounded(true);
                    jumpNum = 0;
                } else if (!isGrounded && jumpNum == 0) // Not grounded when jumping (fall off edge)
                {
                    //playerAnimation.SetGrounded(false);
                    jumpNum = 1;
                }
                if (lastGroundedTime > 0f) // Decrease time when delay started
                {
                    lastGroundedTime -= Time.deltaTime;
                }
                playerAnimation.SetGrounded(isGrounded);
            }
        }
    }

    public bool IsGrounded() { 
        return isGrounded; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    private void Jump(object sender, System.EventArgs e)
    {
        if (jumpNum < jumpMaxCount)
        {
            if (jumpNum == 0) lastGroundedTime = groundCheckDelay;
            jumpNum++;
            isJumping = true;
            jumpDelay = 0.1f;
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); 
            playerAnimation.SetGrounded(false);
            playerAnimation.SetJumping(true);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 positionToMove = axisOfMovement * movement * speed * Time.fixedDeltaTime;
            CalculateRotation(positionToMove);
            //rb.MovePosition(transform.position + positionToMove);
            transform.position += positionToMove;
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        }
    }

    private void CalculateRotation(Vector3 positionToMove)
    {
        if (isRotating && (movement > 0.01f || movement < 0.01f))
        {
            float distanceToMove = movement >= 0f ? positionToMove.magnitude : -1 * positionToMove.magnitude;
            distanceAxis = Mathf.Clamp(distanceAxis + distanceToMove, minClamp, maxClamp);
            axisOfMovement = Vector3.Slerp(startAxis, endAxis, distanceAxis / maxDistanceAxis);
            float yVal = distanceAxis / maxDistanceAxis * targetYRotation;
            if (maxDistanceAxis < 0) yVal = targetYRotation - yVal;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yVal, transform.rotation.eulerAngles.z);
            Transform virtualCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.transform;
            virtualCamera.rotation = Quaternion.Euler(virtualCamera.rotation.eulerAngles.x, yVal, virtualCamera.rotation.eulerAngles.z);
        }
    }

    public void SetAxisMovement(Vector3 axisMovement)
    {
        axisOfMovement = axisMovement;
    }

    public Vector3 GetAxisOfMovement()
    {
        return axisOfMovement;
    }

    public void StartRotation(Vector3 startRotation, Vector3 endRotation, float distanceToCompleteRotation, float rotateY)
    {
        if (Vector3.Distance(axisOfMovement, startRotation) < 0.01f)
        {
            startAxis = startRotation;
            endAxis = endRotation;
            maxDistanceAxis = distanceToCompleteRotation;
            distanceAxis = 0;
            minClamp = 0;
            maxClamp = maxDistanceAxis;
            isRotating = true;
            targetYRotation = rotateY;
        } else if (Vector3.Distance(axisOfMovement, endRotation) < 0.01f)
        {
            startAxis = endRotation;
            endAxis = startRotation;
            maxDistanceAxis = distanceToCompleteRotation * -1;
            distanceAxis = 0;
            minClamp = maxDistanceAxis;
            maxClamp = 0;
            isRotating = true;
            targetYRotation = rotateY;
        } else
        {
            Debug.LogWarning($"PlayerMovement: Rotation Axis doesn't match. start {startRotation} end {endRotation} axisOfMovement {axisOfMovement}");
        }
    }

    public void EndRotation()
    {
        isRotating = false;
    }

    public void SetMovementAllowed(bool movementAllowed)
    {
        canMove = movementAllowed;
    }
}
