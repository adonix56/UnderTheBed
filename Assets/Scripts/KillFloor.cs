using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class KillFloor : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        PlayerRespawn playerRespawn;
        if (other.TryGetComponent<PlayerRespawn>(out playerRespawn))
        {
            Debug.Log("Kill Floor");
            playerRespawn.SetDead();
        }
    }
}
