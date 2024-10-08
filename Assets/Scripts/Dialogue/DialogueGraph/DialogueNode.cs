using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string nodeID;
    public string text;
    public List<string> children;

    public Rect position;
}
