using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnTimer;

    [SerializeField] private bool IsDead;
    private Vector3 respawnPosition;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = transform.position;
    }

    public void SetDead()
    {
        CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Follow = null;
        timer = respawnTimer;
        IsDead = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            } else
            {
                transform.position = respawnPosition;
                IsDead = false;
                CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Follow = transform;
            }
        }
    }
}
