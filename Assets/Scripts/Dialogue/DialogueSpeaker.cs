using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dialogue;
using UnityEngine.UI;

public class DialogueSpeaker : MonoBehaviour
{
    [SerializeField] private Image mainSprite;
    [SerializeField] private Image backSprite;
    [SerializeField] private TextMeshProUGUI nameTag;
    [SerializeField] private TextMeshProUGUI dialogue;

    public void SetNewNode(DialogueNode dialogueNode)
    {
        if (dialogueNode.isLeftSpeaker)
        {
            mainSprite.sprite = dialogueNode.leftSpeaker.GetCharacterSprite(dialogueNode.leftExpression);
            backSprite.sprite = dialogueNode.rightSpeaker.GetCharacterSprite(dialogueNode.rightExpression);
            nameTag.text = dialogueNode.leftSpeaker.characterName;
        }
        else
        {
            mainSprite.sprite = dialogueNode.rightSpeaker.GetCharacterSprite(dialogueNode.rightExpression);
            backSprite.sprite = dialogueNode.leftSpeaker.GetCharacterSprite(dialogueNode.leftExpression);
            nameTag.text = dialogueNode.rightSpeaker.characterName;
        }
        dialogue.text = dialogueNode.text;
    }
}
