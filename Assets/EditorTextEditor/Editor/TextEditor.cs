using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorTextEditor
{
    public class TextEditor : EditorWindow
    {
        // tabs
        private List<FileTab> OpenTabs = new List<FileTab>();
        private FileTab CurrentTab;

        // GUI state
        Vector2 textViewScrollPosition = Vector2.zero;
        Vector2 tabsViewScrollPosition = Vector2.zero;


        public void SelectTab(FileTab fileTab)
        {
            CurrentTab = fileTab;
            GUI.FocusControl(null); // remove focus so the textarea updates with the new file
        }

        public void OnGUI()
        {
            // file buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("New file"))
            {
                var newTab = new FileTab(this);
                OpenTabs.Add(newTab);
                SelectTab(newTab);
            }
            if (GUILayout.Button("Open"))
            {
                OpenFile(EditorUtility.OpenFilePanel("Open text file", "", ""));
            }

            EditorGUI.BeginDisabledGroup(CurrentTab == null);
            if (GUILayout.Button("Save"))
            {
                SaveFile(CurrentTab.filePath, CurrentTab.currentTextContent);
            }
            if (GUILayout.Button("Close"))
            {
                CloseCurrent();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // tabs area
            Color originalBackgroundColor = GUI.backgroundColor;
            tabsViewScrollPosition = GUILayout.BeginScrollView(tabsViewScrollPosition, false, false, GUIStyle.none, GUIStyle.none, GUILayout.ExpandHeight(false));
            EditorGUILayout.BeginHorizontal();
            foreach (FileTab fileTab in OpenTabs)
            {
                fileTab.OnGUI(CurrentTab == fileTab);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUI.backgroundColor = originalBackgroundColor;

            // text editing area
            if (CurrentTab != null)
            {
                textViewScrollPosition = GUILayout.BeginScrollView(textViewScrollPosition);
                UpdateTextContent(EditorGUILayout.TextArea(CurrentTab.currentTextContent, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)));
                GUILayout.EndScrollView();
            }
        }

        private void CloseCurrent()
        {
            if (CurrentTab == null)
            {
                return;
            }
            OpenTabs.Remove(CurrentTab);
            if (OpenTabs.Count > 0)
            {
                SelectTab(OpenTabs[0]);
            }
            else
            {
                CurrentTab = null;
            }
        }

        private void UpdateTextContent(string newTextContent)
        {
            if (newTextContent.Equals(CurrentTab.currentTextContent) == false)
            {
                CurrentTab.currentTextContent = newTextContent;
                CurrentTab.hasEditedSinceSaving = true;
            }
        }

        public void OpenFile(string filePath)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return;
            }

            var newTextContent = File.ReadAllText(filePath);
            var newTab = new FileTab(this);
            newTab.filePath = filePath;
            newTab.currentTextContent = newTextContent;

            OpenTabs.Add(newTab);
            SelectTab(newTab);
        }

        private void SaveFile(string filePath, string fileContent)
        {
            if (filePath == null || filePath.Length < 1)
            {
                // save as new file
                string savePath = EditorUtility.SaveFilePanel("Save file as", CurrentTab.filePath, "newfile.txt", "");
                if (savePath != null && savePath.Length > 0)
                {
                    File.WriteAllText(savePath, fileContent);
                    CurrentTab.hasEditedSinceSaving = false;
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                // overwrite existing file
                File.WriteAllText(filePath, fileContent);
                CurrentTab.hasEditedSinceSaving = false;
                AssetDatabase.Refresh();
            }
        }
    }
}
