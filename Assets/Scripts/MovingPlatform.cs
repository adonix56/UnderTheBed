using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    enum MoveDirection {
        Up, Down, Left, Right
    }

    [SerializeField]
    private MoveDirection moveDirection;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float moveDistance;
    [SerializeField]
    private Vector3 axisOfMovement;
    [SerializeField]
    private bool backAndForth;

    private int forward = 1;
    private float curDistance;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float oldDistance;
    private Transform playerTransform;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position + CalculateMoveVector();
        oldDistance = moveDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveDistance != oldDistance) {
            endPosition = startPosition + CalculateMoveVector();
            oldDistance = moveDistance;
        }
        curDistance = Mathf.Clamp(curDistance + forward * Time.fixedDeltaTime * moveSpeed, 0f, moveDistance + 0.01f);
        transform.position = Vector3.Lerp(startPosition, endPosition, curDistance / moveDistance);
        if (curDistance >= moveDistance || curDistance <= 0.01f)
        {
            if (backAndForth)
            {
                forward = -forward;
            } else
            {
                if (playerTransform != null) {
                    playerTransform.SetParent(null);
                    playerTransform = null;
                }
                transform.position = startPosition;
                curDistance = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            playerTransform = controller.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            playerTransform = controller.transform;
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }

    private Vector3 CalculateMoveVector()
    {
        switch (moveDirection)
        {
            case MoveDirection.Left:
                return -1 * moveDistance * axisOfMovement;
            case MoveDirection.Right:
                return moveDistance * axisOfMovement;
            case MoveDirection.Up:
                return moveDistance * Vector3.up;
            case MoveDirection.Down:
                return moveDistance * Vector3.down;
            default:
                Debug.LogError("MovingPlatform: moveDirection is not a valid value.");
                return Vector3.zero;
        }
    }
}
