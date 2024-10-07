using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Dialogue.Editor
{
    public class DialogueGraph : EditorWindow
    {
        [MenuItem("Window/Dialogue Graph")]
        public static void ShowEditorWindow() {
            GetWindow<DialogueGraph>(false, "Dialogue Graph");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OpenDialogue(int instanceID, int line)
        {
            Debug.Log("Open Dialogue");
            return false;
        }
    }
}
