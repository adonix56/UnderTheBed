using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnTimer;

    [SerializeField] private bool IsDead;
    [SerializeField] private float timer;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private CinemachineVirtualCamera leftFollowCamera;
    [SerializeField] private CinemachineVirtualCamera rightFollowCamera;
    private Vector3 respawnPosition;
    private Quaternion respawnRotation;
    private Quaternion respawnLCameraRotation;
    private Quaternion respawnRCameraRotation;
    private Vector3 respawnAxisMovement;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
        respawnLCameraRotation = leftFollowCamera.VirtualCameraGameObject.transform.rotation;
        respawnRCameraRotation = rightFollowCamera.VirtualCameraGameObject.transform.rotation;
        respawnAxisMovement = playerMovement.GetAxisOfMovement();
    }

    public void SetDead(bool removeSprite = false)
    {
        playerMovement.SetMovementAllowed(false);
        if (removeSprite) playerSprite.SetActive(!removeSprite);
        leftFollowCamera.Follow = null;
        rightFollowCamera.Follow = null;
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
                playerSprite.SetActive(true);
                leftFollowCamera.VirtualCameraGameObject.transform.rotation = respawnLCameraRotation;
                rightFollowCamera.VirtualCameraGameObject.transform.rotation = respawnRCameraRotation;
                IsDead = false;
                leftFollowCamera.Follow = transform;
                rightFollowCamera.Follow = transform;
                playerMovement.SetAxisMovement(respawnAxisMovement);
                playerMovement.SetMovementAllowed(true);
            }
        }
    }
    
    public void SetRespawnPoint()
    {
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
        respawnLCameraRotation = leftFollowCamera.VirtualCameraGameObject.transform.rotation;
        respawnRCameraRotation = rightFollowCamera.VirtualCameraGameObject.transform.rotation;
        respawnAxisMovement = playerMovement.GetAxisOfMovement();
    }
}
