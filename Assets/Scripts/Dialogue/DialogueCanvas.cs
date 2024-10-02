using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    [SerializeField] private GameObject leftSpeaker;
    [SerializeField] private GameObject rightSpeaker;

    public void ActivateDialogue(bool activeLeftSpeaker)
    {
        leftSpeaker.SetActive(activeLeftSpeaker);
        rightSpeaker.SetActive(!activeLeftSpeaker);
    }

    public void DeactivateDialogue()
    {
        leftSpeaker.SetActive(false);
        rightSpeaker.SetActive(false);
    }
}
