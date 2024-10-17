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
        [NonSerialized]
        GUIStyle nodeStyle;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 offset = new Vector2();
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;

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
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNodeConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                ProcessNodeChanges();
                ProcessEvents();
            }
            Repaint();
        }

        private void ProcessEvents()
        {
            if (Event.current.button == 0)
            {
                LeftMouseEvents();
            } else if (Event.current.button == 1)
            {
                RightMouseEvents();
            }
        }

        private void ProcessNodeChanges()
        {
            if (creatingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Added New Dialogue Node");
                selectedDialogue.CreateNewNode(creatingNode);
                creatingNode = null;
            }
            if (deletingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Deleted Dialogue Node");
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }
        }

        private void LeftMouseEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = selectedDialogue.GetNode(Event.current.mousePosition);
                if (draggingNode != null)
                {
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

        private void RightMouseEvents()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                Debug.Log("Right Click Down");
                draggingNode = selectedDialogue.GetNode(Event.current.mousePosition);
                if (draggingNode != null)
                {
                    offset = draggingNode.rectPosition.position - Event.current.mousePosition;
                }
                Event.current.Use();
            } else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Vector3 startPosition = new Vector2(draggingNode.rectPosition.xMax, draggingNode.rectPosition.center.y);
                Vector3 endPosition = Event.current.mousePosition;
                //Debug.Log($"Right Click Drag {startPosition} and {endPosition}");
                DrawConnection(startPosition, endPosition);
                GUI.changed = true;
                Event.current.Use();
            } else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                Debug.Log("Right Click End");
                draggingNode = null;
                Event.current.Use();
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
                node.text = newText;
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawNodeConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.rectPosition.xMax, node.rectPosition.center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetChildren(node)) {
                //EditorGUILayout.LabelField(childNode.text);
                Vector3 endPosition = new Vector2(childNode.rectPosition.xMin, childNode.rectPosition.center.y);
                DrawConnection(startPosition, endPosition);
            }
        }

        private void DrawConnection(Vector3 start, Vector3 end)
        {
            //Debug.Log($"Drawing {start}, {end}");
            Vector3 tangentOffset = new Vector2(Mathf.Abs(start.x - end.x) * 0.6f, 0f);
            Color bezierColor = start.x - end.x > 0 ? Color.red : Color.white;
            Handles.DrawBezier(start, end, start + tangentOffset, end - tangentOffset, bezierColor, null, 4f);
        }
    }
}
