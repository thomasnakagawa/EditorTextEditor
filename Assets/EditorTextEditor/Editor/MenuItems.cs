using UnityEditor;
using UnityEngine;

namespace EditorTextEditor
{
    public class MenuItems
    {
        [MenuItem("Tools/Text editor")]
        public static void OpenWindow()
        {
            EditorWindow.GetWindow(typeof(TextEditor)).Show();
        }

        [MenuItem("Assets/Open in text editor")]
        public static void OpenInEditor()
        {
            OpenWindow();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
            EditorWindow.GetWindow<TextEditor>().OpenFile(path);
        }

        [MenuItem("Assets/Open in text editor", true)]
        public static bool CanOpenInEditor()
        {
            return Selection.activeObject is TextAsset;
        }
    }
}
