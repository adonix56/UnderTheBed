using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvas : MonoBehaviour
{
    [SerializeField] private DialogueSpeaker leftSpeaker;
    [SerializeField] private DialogueSpeaker rightSpeaker;

    private DialogueSO currentDialogue;
    private DialogueNode currentDialogueNode;

    public void SetDialogue(DialogueSO dialogue)
    {
        if (dialogue && currentDialogue != dialogue)
        {
            currentDialogue = dialogue;
            currentDialogueNode = dialogue.nodes[0];
        }
    }

    public void SetNodeDetails()
    {
        if (currentDialogueNode)
        {
            if (currentDialogueNode.isLeftSpeaker)
            {
                leftSpeaker.SetNewNode(currentDialogueNode);
                leftSpeaker.gameObject.SetActive(true);
                rightSpeaker.gameObject.SetActive(false);
                //leftSpeaker.transform.Find("Sprite").GetComponent<Image>().sprite = currentDialogueNode.leftSpeaker.GetCharacterSprite(currentDialogueNode.leftExpression);
                //leftSpeaker.transform.Find("Right Sprite").GetComponent<Image>().sprite = currentDialogueNode.rightSpeaker.GetCharacterSprite(currentDialogueNode.rightExpression);
            } else
            {
                rightSpeaker.SetNewNode(currentDialogueNode);
                leftSpeaker.gameObject.SetActive(false);
                rightSpeaker.gameObject.SetActive(true);
                //rightSpeaker.transform.Find("Sprite").GetComponent<Image>().sprite = currentDialogueNode.rightSpeaker.GetCharacterSprite(currentDialogueNode.rightExpression);
                //rightSpeaker.transform.Find("Left Sprite").GetComponent<Image>().sprite = currentDialogueNode.leftSpeaker.GetCharacterSprite(currentDialogueNode.leftExpression);
            }
        } else
        {
            DeactivateDialogue();
        }
    }

    public void ActivateDialogue(bool alreadyActive)
    {
        if (alreadyActive)
        {
            currentDialogueNode = currentDialogueNode.children.Count > 0 ? currentDialogue.GetNode(currentDialogueNode.children[0]) : null;
            
        }
        SetNodeDetails();
    }

    public void DeactivateDialogue()
    {
        leftSpeaker.gameObject.SetActive(false);
        rightSpeaker.gameObject.SetActive(false);
    }

    public bool HasDialogue()
    {
        return currentDialogue && currentDialogueNode;
    }
}
