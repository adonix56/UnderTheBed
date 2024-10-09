using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Objects/Dialogue", order = 0)]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        private List<DialogueNode> nodes;

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif
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
    }
}
