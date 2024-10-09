using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [HideInInspector]
    public string nodeID;
    public string text;
    public List<string> children = new List<string>();

    public Rect rectPosition = new Rect(0, 0, 200, 100);

}
