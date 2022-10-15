using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace PokemonGame.Editor
{
    public class RegisterEditorWindow : EditorWindow
    {
        [MenuItem("Window/Register Editor")]
        public static void ShowWindow()
        {
            RegisterEditorWindow window = GetWindow<RegisterEditorWindow>("Register Editor");
        }

        Rect buttonRect;
        private VisualElement leftPane;
        private VisualElement rightPane;

        public void OpenNewScreen(RegisterViewId id)
        {
            rootVisualElement.Clear();
        }
        
        private void OnGUI()
        {
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        
            rootVisualElement.Add(splitView);
        
            leftPane = new VisualElement();
            splitView.Add(leftPane);
            rightPane = new VisualElement();
            splitView.Add(rightPane);
            
            GUILayout.Label("Select which items to view", EditorStyles.boldLabel);
            if (GUILayout.Button("Select Option", GUILayout.Width(200)))
            {
                PopupWindow.Show(buttonRect, new PopupExample(this));
            }
            if (Event.current.type == EventType.Repaint)
            {
                buttonRect = GUILayoutUtility.GetLastRect();
            }
        }
    }

    public enum RegisterViewId
    {
        AllItems,
        AllAis,
        AllMoves,
        AllStatusEffects
    }
}