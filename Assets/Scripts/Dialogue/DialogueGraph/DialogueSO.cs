using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Objects/Dialogue", order = 0)]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        private List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField]
        private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(CreateNewNode());
            }
        }
#endif
        public void OnValidate()
        {
            nodeLookup.Clear();

            foreach (DialogueNode node in nodes)
            {
                nodeLookup[node.nodeID] = node;
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

        public DialogueNode CreateNewNode(DialogueNode parent = null)
        {
            DialogueNode newNode = new DialogueNode();
            newNode.nodeID = Guid.NewGuid().ToString();
            nodes.Add(newNode);
            nodeLookup[newNode.nodeID] = newNode;
            if (parent != null)
                parent.children.Add(newNode.nodeID);
            return newNode;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            OnValidate();
            foreach (DialogueNode node in nodes)
            {
                node.children.Remove(nodeToDelete.nodeID);
            }
        }
    }
}
