using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private CharacterSO leftCharacter;
    [SerializeField] private CharacterSO rightCharacter;

    //TODO: Testing purposes, remove later
    [SerializeField] private DialogueCanvas dialogueCanvas;
    bool alternateLeftSpeaker = true;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController controller;
        if (other.TryGetComponent<PlayerController>(out controller))
        {
            controller.InteractPressed += Interact;
            interactCanvas.SetActive(true);
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
            }
        }
    }

    private void Interact(object sender, System.EventArgs e) 
    {
        // TODO: Testing purposes, remove later
        if (dialogueCanvas)
        {
            dialogueCanvas.ActivateDialogue(alternateLeftSpeaker);
            dialogueCanvas.SetSpeakers(leftCharacter, rightCharacter);
            alternateLeftSpeaker = !alternateLeftSpeaker;
        }
    }
}
