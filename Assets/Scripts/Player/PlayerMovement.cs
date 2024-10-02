using System.Collections;
using System.Collections.Generic;
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
    private bool isGrounded;
    private int jumpNum;
    private float movement;
    private Vector3 axisOfMovement;
    private PlayerController playerController;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.JumpPressed += Jump;
        rb = GetComponent<Rigidbody>();
        //TEST
        axisOfMovement = new Vector3(1f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        movement = playerController.GetMoveInput();
        isGrounded = Physics.CheckBox(transform.position, boxSize, transform.rotation, platformMask); 
        if (isGrounded && lastGroundedTime <= 0f) // Grounded after delay
        {
            jumpNum = 0;
        }
        else if (!isGrounded && jumpNum == 0) // Not grounded when jumping (fall off edge)
        {
            jumpNum = 1;
        }
        if (lastGroundedTime > 0f) // Decrease time when delay started
        {
            lastGroundedTime -= Time.deltaTime;
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
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + axisOfMovement * movement * speed * Time.fixedDeltaTime);
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass); 
    }
}
