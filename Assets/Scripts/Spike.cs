using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    //checks collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerRespawn>(out PlayerRespawn player))
        {
            player.SetDead(true);
        }

    }
}
