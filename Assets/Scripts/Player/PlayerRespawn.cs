using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnTimer;

    [SerializeField] private bool IsDead;
    [SerializeField] private float timer;
    private ICinemachineCamera followCamera;
    private Vector3 respawnPosition;
    private Quaternion respawnRotation;
    private Quaternion respawnCameraRotation;
    private Vector3 respawnAxisMovement;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
        followCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        respawnCameraRotation = followCamera.VirtualCameraGameObject.transform.rotation;
        respawnAxisMovement = playerMovement.GetAxisOfMovement();
    }

    public void SetDead()
    {
        followCamera.Follow = null;
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
                transform.rotation = respawnRotation;
                followCamera.VirtualCameraGameObject.transform.rotation = respawnCameraRotation;
                IsDead = false;
                followCamera.Follow = transform;
                playerMovement.SetAxisMovement(respawnAxisMovement);
            }
        }
    }
}
