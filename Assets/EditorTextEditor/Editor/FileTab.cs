using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorTextEditor
{
    public class FileTab
    {
        public FileTab(TextEditorWindow editor)
        {
            this.editor = editor;
        }

        private TextEditorWindow editor;

        public string filePath = "untitled";
        public bool hasEditedSinceSaving = false;
        public string currentTextContent = "placeholder";

        public void OnGUI(bool isCurrentlySelected)
        {
            if (isCurrentlySelected)
            {
                GUI.backgroundColor = new Color(0.8f, 0.8f, 1f);
            }
            string filename = filePath.Split(Path.DirectorySeparatorChar).Last();
            if (hasEditedSinceSaving)
            {
                filename += "*";
            }
            if (GUILayout.Button(filename, EditorStyles.toolbarButton))
            {
                editor.SelectTab(this);
            }
            GUI.backgroundColor = Color.white;
        }
    }
}
