using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{

    //[System.Serializable]
    public class DialogueNode : ScriptableObject
    {
        [HideInInspector] public string nodeID;
        [HideInInspector] public Rect rectPosition;
        [HideInInspector] public DialogueSO parentDialogue;
        public CharacterSO leftSpeaker;
        public CharacterSO.CharacterExpression leftExpression;
        public CharacterSO rightSpeaker;
        public CharacterSO.CharacterExpression rightExpression;
        public bool isLeftSpeaker;
        [TextArea(3, 10)]
        public string text;
        public List<string> children = new List<string>();

        public void Initialize(DialogueSO dialogueSO)
        {
            Initialize(dialogueSO, new Rect(0, 0, 200, 100));
        }

        public void Initialize(DialogueSO dialogueSO, Rect rect)
        {
            parentDialogue = dialogueSO;
            leftSpeaker = dialogueSO.leftSpeaker;
            rightSpeaker = dialogueSO.rightSpeaker;
            rectPosition = rect;
            SetName();
        }

        private void OnValidate()
        {
            SetName();
        }

        private void SetName()
        {
            if (isLeftSpeaker && leftSpeaker)
            {
                name = leftSpeaker.characterName;
            } else if (!isLeftSpeaker && rightSpeaker)
            {
                name = rightSpeaker.characterName;
            } else {
                name = "New Dialogue Node";
            }
        }

        public void ToggleChild(DialogueNode newChild)
        {
            if (!children.Contains(newChild.nodeID)) children.Add(newChild.nodeID);
            else children.Remove(newChild.nodeID);
        }
    }
}
