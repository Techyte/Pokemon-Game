using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

// You can go to Assets > Create > Demo Asset to create an asset of this type, and so check the inspector view.
[CreateAssetMenu(fileName = "NewDemoAsset", menuName = "Demo Asset")]
public class DemoAsset : ScriptableObject
{
    public string[] playerNames = { };
}

// This part should be in a separate file. I grouped the asset and its editor for convenience here.
[CustomEditor(typeof(DemoAsset))]
public class DemoAssetEditor : Editor
{
    [SerializeField]
    private int _selectedItem = -1;

    private ReorderableList _playerNamesList = null;

    public override void OnInspectorGUI()
    {
        // Use the getter, not the field
        PlayerNamesList.DoLayoutList();
    }

    private ReorderableList PlayerNamesList
    {
        get
        {
            // Create the reorderable list if it doesn't exist
            if (_playerNamesList == null)
            {
                _playerNamesList = new ReorderableList(serializedObject, serializedObject.FindProperty("playerNames"));

                _playerNamesList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Player Names", EditorStyles.boldLabel);
                
                // When an item is selected, update the folded out item index, and kill the list so it can be redrawn properly
                _playerNamesList.onSelectCallback += list =>
                {
                    _selectedItem = list.index;
                    //_playerNamesList = null; // Toggle this line to see how the editor behaves by default without killing the list
                };

                // Set the element height to 200px when selected, or default height otherwise
                _playerNamesList.elementHeightCallback += index => _selectedItem == index ? 200f : EditorGUIUtility.singleLineHeight;

                _playerNamesList.drawElementCallback += (rect, index, isActive, isFocused) =>
                {
                    if (_selectedItem == index)
                    {
                        Rect tmpRect = new Rect(rect);
                        tmpRect.height = EditorGUIUtility.singleLineHeight;
                        EditorGUI.Foldout(tmpRect, true, "Element " + index);

                        tmpRect.y += tmpRect.height + 2f;
                        tmpRect.height = rect.height - tmpRect.height - 2f;
                        EditorGUI.DrawRect(tmpRect, Color.black);
                    }
                    else
                        EditorGUI.Foldout(rect, false, "Element " + index);
                };
            }
            return _playerNamesList;
        }
    }
}