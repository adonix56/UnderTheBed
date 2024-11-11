using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPlatform : MonoBehaviour
{
    private const string FLASH = "Flash";
    private Animator animator;
    private BoxCollider myBox;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myBox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger(FLASH);
        myBox.enabled = false;
    }
}
