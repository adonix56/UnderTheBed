using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPusher : MonoBehaviour
{
    //allows edit in inspector
    [SerializeField]
    private float forceStrength;

    //checks for collision with object
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //gets rigid body
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        //checks if there is a valid object that is being colliding with player
        if (rigidbody != null)
        {
            //gets direction force should be applied
            Vector2 forceDirection = hit.gameObject.transform.position - transform.position;
            //prevents upward movement
            forceDirection.y = 0;
            forceDirection.Normalize();

            //applies force from player position to object
            rigidbody.AddForceAtPosition(forceDirection * forceStrength, transform.position, ForceMode.Impulse);
        }
    }
}
