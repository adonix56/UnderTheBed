using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvas : MonoBehaviour
{
    [SerializeField] private GameObject leftSpeaker;
    [SerializeField] private GameObject rightSpeaker;

    public void SetSpeakers(CharacterSO left, CharacterSO right)
    {
        leftSpeaker.transform.Find("Sprite").GetComponent<Image>().sprite = left.Neutral;
        leftSpeaker.transform.Find("Right Sprite").GetComponent<Image>().sprite = right.Neutral;
        rightSpeaker.transform.Find("Sprite").GetComponent<Image>().sprite = right.Neutral;
        rightSpeaker.transform.Find("Left Sprite").GetComponent<Image>().sprite = left.Neutral;
    }

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
