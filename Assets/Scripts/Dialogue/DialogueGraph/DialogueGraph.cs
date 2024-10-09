using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Dialogue.Editor
{
    public class DialogueGraph : EditorWindow
    {
        DialogueSO selectedDialogue = null;
        GUIStyle nodeStyle;
        DialogueNode draggingNode = null;
        Vector2 offset = new Vector2();

        [MenuItem("Window/Dialogue Graph")]
        public static void ShowEditorWindow() {
            GetWindow<DialogueGraph>(false, "Dialogue Graph");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            DialogueSO dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Debug.Log("Selection Changed");
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("No Dialogue Selected");
            } else
            {
                ProcessEvents();
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = selectedDialogue.GetNode(Event.current.mousePosition);
                if (draggingNode != null) {
                    offset = draggingNode.rectPosition.position - Event.current.mousePosition;
                }
            } else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Dialogue Node Reposition");
                draggingNode.rectPosition.position = Event.current.mousePosition + offset;
                GUI.changed = true;
            } else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            } 
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rectPosition, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(node.nodeID);
            string newText = EditorGUILayout.TextField(node.text);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Dialogue Text Changed");
                node.nodeID = newNodeID;
                node.text = newText;
            }
            GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.rectPosition.xMax, node.rectPosition.center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetChildren(node)) {
                //EditorGUILayout.LabelField(childNode.text);
                Vector3 endPosition = new Vector2(childNode.rectPosition.xMin, childNode.rectPosition.center.y);
                DrawNodeConnection(startPosition, endPosition);
            }
        }

        private void DrawNodeConnection(Vector3 start, Vector3 end)
        {
            Vector3 tangentOffset = new Vector2(Mathf.Abs(start.x - end.x) * 0.6f, 0f);
            Color bezierColor = start.x - end.x > 0 ? Color.red : Color.white;
            Handles.DrawBezier(start, end, start + tangentOffset, end - tangentOffset, bezierColor, null, 4f);
        }
    }
}
