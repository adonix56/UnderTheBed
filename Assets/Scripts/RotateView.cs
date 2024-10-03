using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateView : MonoBehaviour
{
    [SerializeField] private Vector3 axisOnEntry;
    [SerializeField] private Vector3 axisOnExit;
    [SerializeField] private float targetYRotation;
    [SerializeField] private float distanceToCompleteRotation;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement;
        if (other.TryGetComponent<PlayerMovement>(out playerMovement))
        {
            //if (playerMovement.GetAxisOfMovement() == axisOnEntry)
            //{
                playerMovement.StartRotation(axisOnEntry, axisOnExit, distanceToCompleteRotation, targetYRotation);
            //} 
        }
    }

    private void OnTriggerExit(Collider other) {
        PlayerMovement playerMovement;
        if (other.TryGetComponent<PlayerMovement>(out playerMovement))
        {
            playerMovement.EndRotation();
        }
    }
}
