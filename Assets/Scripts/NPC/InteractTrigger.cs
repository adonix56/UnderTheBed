using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private DialogueSO dialogue;

    //TODO: Testing purposes, remove later
    [SerializeField] private DialogueCanvas dialogueCanvas;
    private bool dialogueActive;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller;
        if (other.TryGetComponent<PlayerController>(out controller))
        {
            controller.GetComponent<PlayerRespawn>().SetRespawnPoint();
            if (dialogue && dialogueCanvas)
            {
                dialogueCanvas.SetDialogue(dialogue);
                if (dialogueCanvas.HasDialogue())
                {
                    controller.InteractPressed += Interact;
                    interactCanvas.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController controller;
        if (other.TryGetComponent<PlayerController>(out controller))
        {
            controller.InteractPressed -= Interact;
            interactCanvas.SetActive(false);

            //TODO: Testing purposes
            if (dialogueCanvas)
            {
                dialogueCanvas.DeactivateDialogue();
                dialogueActive = false;
            }
        }
    }

    private void Interact(object sender, System.EventArgs e) 
    {
        // TODO: Testing purposes, remove later
        if (dialogueCanvas && dialogueCanvas.HasDialogue())
        {
            dialogueCanvas.ActivateDialogue(dialogueActive);
            dialogueActive = true;
            if (!dialogueCanvas.HasDialogue())
            {
                dialogueActive = false;
                interactCanvas.SetActive(false);
            }
        }
    }
}
