using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravityScale = 5f;
    
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
    }

    private void Jump(object sender, System.EventArgs e)
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        Debug.Log("Jumping");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + axisOfMovement * movement * speed * Time.fixedDeltaTime);
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
    }
}
