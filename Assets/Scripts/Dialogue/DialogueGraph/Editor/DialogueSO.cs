using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using UnityEditor;

namespace Dialogue.Editor
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Objects/Dialogue", order = 0)]
    public class DialogueSO : ScriptableObject
    {
        public CharacterSO leftSpeaker;
        public CharacterSO rightSpeaker;
        [SerializeField]
        public List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField]
        private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnEnable()
        {
            if (nodes.Count == 0)
            {
                CreateNewNode();
            }
        }

        public void OnValidate()
        {
            nodeLookup.Clear();

            foreach (DialogueNode node in nodes)
            {
                nodeLookup[node.nodeID] = node;
                node.leftSpeaker = leftSpeaker;
                node.rightSpeaker = rightSpeaker;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes() {
            return nodes;
        }

        public DialogueNode GetNode(Vector2 mousePosition)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].rectPosition.Contains(mousePosition))
                {
                    return nodes[i];
                }
            }
            return null;
        }

        public IEnumerable<DialogueNode> GetChildren(DialogueNode node)
        {
            foreach (string child in node.children)
            {
                if (!nodeLookup.ContainsKey(child)) 
                    yield return null;
                else 
                    yield return nodeLookup[child];
            }
        }

        public void CreateNewNode(DialogueNode parent = null)
        {
            //DialogueNode newNode = new DialogueNode();
            DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
            newNode.nodeID = Guid.NewGuid().ToString();
            nodes.Add(newNode);
            nodeLookup[newNode.nodeID] = newNode;
            if (parent != null)
            {
                parent.children.Add(newNode.nodeID);
                Rect newRect = parent.rectPosition;
                newRect.x += newRect.width * 1.3f;
                newNode.Initialize(this, newRect);
            } else
            {
                newNode.Initialize(this);
            }
            AssetDatabase.AddObjectToAsset(newNode, this);
            AssetDatabase.SaveAssets();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            DestroyImmediate(nodeToDelete, true);
            OnValidate();
            foreach (DialogueNode node in nodes)
            {
                node.children.Remove(nodeToDelete.nodeID);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
