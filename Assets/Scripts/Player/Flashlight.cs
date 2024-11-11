using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private CubismModel CubismModel;
    [SerializeField] private float speed;
    [SerializeField] private Transform flashlightBase;

    private PlayerController playerController;
    private MeshRenderer flashlightRenderer;
    public bool FlashlightActive { get; private set; }
    public float FlashlightDot { get; private set; }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.FlashlightStarted += FlashlightOn;
        playerController.FlashlightEnded += FlashlightOff;

        flashlightRenderer = flashlightBase.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        flashlightRenderer.sortingLayerName = "Test";
        //flashlightRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(flashlightBase.position);
        Vector3 flashlightAim = playerController.GetFlashlightAim();
        flashlightAim.z = screenPos.z;
        flashlightAim -= screenPos;
        FlashlightDot = Vector3.Dot(Vector3.right, flashlightAim);
        Quaternion flashlightRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(flashlightAim.y, flashlightAim.x) * Mathf.Rad2Deg);
        flashlightBase.rotation = Quaternion.RotateTowards(flashlightBase.rotation, flashlightRotation, speed * Time.deltaTime);

        //float newValue = CubismModel.Parameters[9].Value + speed * Time.deltaTime;
        //if (newValue > 180) newValue -= 180;
        //CubismModel.Parameters[9].Value = newValue;
        //CubismModel.Parameters[10].Value = 1f;
    }

    private void FlashlightOn(object sender, System.EventArgs e)
    {
        flashlightRenderer.gameObject.SetActive(true);
        FlashlightActive = true;
        //flashlightRenderer.enabled = true;
    }

    private void FlashlightOff(object sender, System.EventArgs e)
    {
        flashlightRenderer.gameObject.SetActive(false);
        FlashlightActive = false;
        //flashlightRenderer.enabled = false;
    }
}
