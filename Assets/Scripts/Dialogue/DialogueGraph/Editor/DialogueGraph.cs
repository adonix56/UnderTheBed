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
        GUIStyle selectedNodeStyle;
        [NonSerialized]
        DialogueNode selectedNode = null;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 offset = new Vector2();
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        Vector3 rightClickDragStart = new Vector3();
        [NonSerialized]
        Vector3 rightClickDragEnd = new Vector3();
        [NonSerialized]
        bool rightClickDrag = false;

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

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("node0 on") as Texture2D;
            selectedNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            //DrawConnection(new Vector3(499f, 147f), new Vector3(1027f, 231f));
            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("No Dialogue Selected");
            } else
            {
                if (rightClickDrag)
                {
                    DrawConnection(rightClickDragStart, rightClickDragEnd);
                }
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
                    Selection.activeObject = draggingNode;
                    selectedNode = draggingNode;
                    offset = draggingNode.rectPosition.position - Event.current.mousePosition;
                } else
                {
                    selectedNode = null;
                    Selection.activeObject = selectedDialogue;
                    offset = Event.current.mousePosition;
                }
            } else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Dialogue Node Reposition");
                draggingNode.rectPosition.position = Event.current.mousePosition + offset;
                GUI.changed = true;
            } else if (Event.current.type == EventType.MouseDrag && draggingNode == null) 
            {
                DraggingGraph();
            } else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
        }

        private void DraggingGraph()
        {
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                node.rectPosition.position += Event.current.mousePosition - offset;
            }
            offset = Event.current.mousePosition;
        }

        private void RightMouseEvents()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                selectedNode = selectedDialogue.GetNode(Event.current.mousePosition);
                if (selectedNode)
                {
                    Selection.activeObject = selectedNode;
                } else
                {
                    Selection.activeObject = selectedDialogue;
                }
                /*if (draggingNode != null)
                {
                    selectedNode = draggingNode;
                    offset = draggingNode.rectPosition.position - Event.current.mousePosition;
                } else
                {
                    selectedNode = null;
                }*/
                Event.current.Use();
            } else if (Event.current.type == EventType.MouseDrag && selectedNode != null)
            {
                rightClickDragStart = new Vector2(selectedNode.rectPosition.xMax, selectedNode.rectPosition.center.y);
                rightClickDragEnd = Event.current.mousePosition;
                rightClickDrag = true;
                GUI.changed = true;
                Event.current.Use();
            } else if (Event.current.type == EventType.MouseUp)
            {
                DialogueNode endNode = selectedDialogue.GetNode(Event.current.mousePosition);
                if (selectedNode && endNode)
                {
                    if (endNode == selectedNode)
                    {
                        ShowContextMenu(true);
                    } else
                    {
                        SetParenting(endNode, selectedNode);
                    }
                } else
                {
                    ShowContextMenu();
                }
                //draggingNode = null;
                rightClickDrag = false;
                Event.current.Use();
            }
        }

        private void ShowContextMenu(bool withDelete = false)
        {
            GenericMenu contextMenu = new GenericMenu();
            if (selectedNode)
            {
                contextMenu.AddItem(new GUIContent("Create Child Node"), false, () => { selectedDialogue.CreateNewNode(selectedNode); });
            } else
            {
                contextMenu.AddItem(new GUIContent("Create New Node"), false, () => { selectedDialogue.CreateNewNode(); });
            }
            if (withDelete) contextMenu.AddItem(new GUIContent("Delete Node"), false, () => { deletingNode = selectedNode; });

            contextMenu.ShowAsContext();
        }

        private void SetParenting(DialogueNode nodeA, DialogueNode nodeB)
        {
            if (nodeA.rectPosition.x < nodeB.rectPosition.x) nodeA.ToggleChild(nodeB); //A parent, B child
            else nodeB.ToggleChild(nodeA); //B parent, A child
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle curStyle = node == selectedNode ? selectedNodeStyle : nodeStyle;
            GUILayout.BeginArea(node.rectPosition, curStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(node.name);
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
            Color bezierColor = Color.white;// start.x - end.x > 0 ? Color.red : Color.white;
            Handles.DrawBezier(start, end, start + tangentOffset, end - tangentOffset, bezierColor, null, 4f);
        }
    }
}
