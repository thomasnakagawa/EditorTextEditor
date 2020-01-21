using UnityEditor;
using UnityEngine;

namespace EditorTextEditor
{
    public static class MenuItems
    {
        [MenuItem("Tools/Text editor")]
        public static void OpenWindow()
        {
            EditorWindow.GetWindow(typeof(TextEditorWindow), false, "Text Editor").Show();
        }

        [MenuItem("Assets/Open in text editor")]
        public static void OpenFileInEditor()
        {
            OpenWindow();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
            EditorWindow.GetWindow<TextEditorWindow>().OpenFile(path);
        }

        [MenuItem("Assets/Open in text editor", true)]
        public static bool CanOpenFileInEditor()
        {
            return Selection.activeObject is TextAsset;
        }
    }
}
